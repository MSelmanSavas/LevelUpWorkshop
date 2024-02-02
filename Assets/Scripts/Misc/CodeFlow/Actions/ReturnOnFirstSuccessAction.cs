using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codeflow.Actions.Composites
{
    [System.Serializable]
    public class ReturnOnFirstSuccessAction : CodeflowBaseAction
    {
        [field: SerializeReference]
        public List<ICodeflowAction> Actions { get; private set; }
        public ICodeflowAction CurrentAction { get; private set; }
        bool _alreadyProcessedOneTimeExecuteForCurrentAction = false;
        int _currentActionIndex = 0;

        public override CodeflowState Execute()
        {
            if (Actions.Count <= 0)
                return CodeflowState.Completed;

            try
            {
                for (int i = _currentActionIndex; i < Actions.Count; i++)
                {
                    CurrentAction = Actions[_currentActionIndex];
                    CodeflowState oneTimeExecuteState = GetOneTimeExecuteState();

                    if (oneTimeExecuteState == CodeflowState.Failed)
                    {
                        _currentActionIndex++;
                        _alreadyProcessedOneTimeExecuteForCurrentAction = false;
                        continue;
                    }

                    CodeflowState executeState = GetExecuteState();

                    switch (executeState)
                    {
                        case CodeflowState.Failed:
                            {
                                _currentActionIndex++;
                                _alreadyProcessedOneTimeExecuteForCurrentAction = false;
                                continue;
                            }
                        case CodeflowState.Executing:
                        case CodeflowState.Waiting:
                            {
                                return executeState;
                            }
                        case CodeflowState.Success:
                            {
                                return CodeflowState.Completed;
                            }
                    }
                }
            }
            catch (System.Exception e)
            {
                Logger.LogErrorWithTag(LogCategory.Codeflow, $"Error while trying to execute {nameof(ReturnOnFirstSuccessAction)}! Error : {e}");
                return CodeflowState.Failed;
            }

            return CodeflowState.Completed;
        }

        CodeflowState GetOneTimeExecuteState()
        {
            if (_alreadyProcessedOneTimeExecuteForCurrentAction)
                return CodeflowState.Success;

            var oneTimeExecuteStatus = CurrentAction.OneTimeExecute();

            switch (oneTimeExecuteStatus)
            {
                case CodeflowState.Failed:
                    {
                        Logger.LogWithTag(LogCategory.Codeflow, $"{CurrentAction} has failed to one time execute! Trying to execute next one!");

                        break;
                    }
                case CodeflowState.Success:
                    {
                        _alreadyProcessedOneTimeExecuteForCurrentAction = true;
                        break;
                    }
            }

            return oneTimeExecuteStatus;
        }

        CodeflowState GetExecuteState()
        {
            var executeStatus = CurrentAction.Execute();

            switch (executeStatus)
            {
                case CodeflowState.Failed:
                    {
                        Logger.LogWithTag(LogCategory.Codeflow, $"{CurrentAction} has failed to execute! Trying to execute next one!");
                        break;
                    }
                case CodeflowState.Success:
                    {
                        _alreadyProcessedOneTimeExecuteForCurrentAction = true;
                        break;
                    }
            }

            return executeStatus;
        }


        public override CodeflowState Reset()
        {
            _currentActionIndex = 0;
            _alreadyProcessedOneTimeExecuteForCurrentAction = false;
            return CodeflowState.Success;
        }
    }
}