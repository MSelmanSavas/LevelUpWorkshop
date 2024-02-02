using System.Collections.Generic;
using Codeflow.Actions;
using Codeflow.Conditions;
using UnityEngine;
using UsefulExtensions.List;

namespace Codeflow.Execution
{
    [System.Serializable]
    public class DefaultCodeflowBaseExecutor : ICodeflowExecutor
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public bool IsInitialized { get; private set; }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public CodeflowState State { get; private set; } = CodeflowState.NoActionStarted;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public CodeflowState CurrentActionState { get; private set; } = CodeflowState.NoActionStarted;

        [field: SerializeReference]
        public ICodeflowConditionsEvaluator ConditionsEvaluator { get; private set; }

        [field: SerializeReference]
        public List<ICodeflowCondition> Conditions { get; private set; }

        [field: SerializeReference]
        public List<ICodeflowAction> Actions { get; private set; }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public ICodeflowAction CurrentAction { get; private set; }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public int CurrentActionIndex { get; private set; }

        public CodeflowState Initialize()
        {
            IsInitialized = false;

            if (ConditionsEvaluator == null)
            {
                Logger.LogErrorWithTag(LogCategory.Codeflow, $"ConditionsEvaluator is null! Not initializing : {this}");
                return CodeflowState.Failed;
            }

            for (int i = 0; i < Conditions.Count; i++)
            {
                if (Conditions[i] == null)
                {
                    Logger.LogErrorWithTag(LogCategory.Codeflow, $"There is/are null/nulls in conditions! Not initializing : {this}");
                    return CodeflowState.Failed;
                }
            }

            if (Actions.Count <= 0)
            {
                Logger.LogErrorWithTag(LogCategory.Codeflow, $"There are no actions to run! Not initializing : {this}");
                return CodeflowState.Failed;
            }

            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i] == null)
                {
                    Logger.LogErrorWithTag(LogCategory.Codeflow, $"There is/are null/nulls in actions! Not initializing : {this}");
                    return CodeflowState.Failed;
                }
            }

            foreach (var condition in Conditions)
            {
                if (condition.Initialize() is CodeflowState.Failed)
                {
                    Logger.LogErrorWithTag(LogCategory.Codeflow, $"Error while initializing condition : {condition}! Cannot initialize Executor : {this}!...");
                    return CodeflowState.Failed;
                }
            }

            foreach (var action in Actions)
            {
                if (action.Initialize() is CodeflowState.Failed)
                {
                    Logger.LogErrorWithTag(LogCategory.Codeflow, $"Error while initializing action : {action}! Cannot initialize Executor : {this}!...");
                    return CodeflowState.Failed;
                }
            }

            CurrentActionIndex = 0;
            IsInitialized = true;
            return CodeflowState.Success;
        }

        public CodeflowState TryStart()
        {
            if (State != CodeflowState.Completed
            && State != CodeflowState.NoActionStarted)
                return CodeflowState.Failed;

            try
            {
                UnityEngine.Profiling.Profiler.BeginSample($"{ConditionsEvaluator.GetType()}");

                if (ConditionsEvaluator.CheckConditions(Conditions) != CodeflowState.Success)
                    return CodeflowState.Failed;
            }
            catch (System.Exception e)
            {
                Logger.LogErrorWithTag(LogCategory.Codeflow, $"Error while evaluating conditions on {this}! Failing TryStart! Error : {e}");
                return CodeflowState.Failed;
            }
            finally
            {
                UnityEngine.Profiling.Profiler.EndSample();
            }

            try
            {
                foreach (var action in Actions)
                {
                    UnityEngine.Profiling.Profiler.BeginSample($"{ConditionsEvaluator.GetType()}");

                    action.Reset();

                    UnityEngine.Profiling.Profiler.EndSample();
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                Logger.LogErrorWithTag(LogCategory.Codeflow, $"Error while resetting actions on {this}! Failing TryStart! Error : {e}");
                return CodeflowState.Failed;
            }

            State = CodeflowState.Executing;

            CurrentAction = Actions.GetFirst();

            CurrentActionState = CodeflowState.Started;

            return CodeflowState.Success;
        }

        public CodeflowState Reset()
        {
            ConditionsEvaluator.Reset();

            foreach (var condition in Conditions)
                condition.Reset();

            foreach (var action in Actions)
                action.Reset();

            State = CodeflowState.NoActionStarted;
            CurrentActionState = CodeflowState.NoActionStarted;

            CurrentActionIndex = 0;
            return CodeflowState.Success;
        }

        public CodeflowState TryProcess()
        {
            if (State is CodeflowState.Completed or CodeflowState.NoActionStarted or CodeflowState.Failed)
                return State;

            if (OneTimeExecuteAction() is CodeflowState.Failed)
            {
                State = CodeflowState.Failed;
                return State;
            }

            if (ProcessAction() is CodeflowState.Failed)
            {
                State = CodeflowState.Failed;
                return State;
            }

            if (CurrentActionState is CodeflowState.Executing or CodeflowState.Failed)
                return State;

            switch (TryLoadNextStep())
            {
                case CodeflowState.Executing:
                    {
                        return TryProcess();
                    }
                case CodeflowState.Failed:
                    {
                        return State;
                    }
                default:
                    {
                        return CodeflowState.Completed;
                    }
            }
        }

        CodeflowState OneTimeExecuteAction()
        {
            try
            {
                if (CurrentActionState is not CodeflowState.Started)
                    return CodeflowState.Success;

                UnityEngine.Profiling.Profiler.BeginSample($"{CurrentAction?.GetType()} One Time Execute");
                CurrentActionState = CurrentAction.OneTimeExecute();

                return CurrentActionState;
            }
            catch (System.Exception e)
            {
                Logger.LogErrorWithTag(LogCategory.Codeflow, $"Error while trying to one time execute action :{CurrentAction}! Returning Failed! Error : {e}");
                return CurrentActionState = CodeflowState.Failed;
            }
            finally
            {
                UnityEngine.Profiling.Profiler.EndSample();
            }
        }

        CodeflowState ProcessAction()
        {
            try
            {
                UnityEngine.Profiling.Profiler.BeginSample($"{CurrentAction?.GetType()} Execute");
                CurrentActionState = CurrentAction.Execute();
            }
            catch (System.Exception e)
            {
                Logger.LogErrorWithTag(LogCategory.Codeflow, $"Error while trying to execute action :{CurrentAction}! FailedWithBreak called, cannot continue execution! Error : {e}");
                State = CodeflowState.Failed;
                CurrentActionState = CodeflowState.Failed;
            }
            finally
            {
                UnityEngine.Profiling.Profiler.EndSample();
            }

            return State;
        }

        CodeflowState TryLoadNextStep()
        {
            CurrentActionIndex++;

            if (CurrentActionIndex >= Actions.Count)
            {
                State = CodeflowState.Completed;
                return State;
            }

            if (Actions[CurrentActionIndex] == null)
            {
                State = CodeflowState.Failed;
                Logger.LogErrorWithTag(LogCategory.Codeflow, $"There was a null action in {this}! Cannot continue executing...");
                return State;
            }

            CurrentAction = Actions[CurrentActionIndex];

            CurrentActionState = CodeflowState.Started;
            return State;
        }
    }
}

