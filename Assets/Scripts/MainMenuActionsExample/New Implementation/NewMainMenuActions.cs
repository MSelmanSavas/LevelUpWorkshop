using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.MainMenu.New
{
    public class NewMainMenuActions : MonoBehaviour
    {
        [field: SerializeReference]
        public List<IAction> IterationsContainer { get; private set; }
        public List<IAction> CurrentIterations { get; private set; }
        public IAction CurrentIteration { get; private set; }
        public int CurrentIterationIndex { get; private set; } = 0;
        public bool HasIterationsInitialized { get; private set; } = false;

        void InitializeActions()
        {
            if (HasIterationsInitialized)
                return;

            bool isAnyErrorHappened = false;

            foreach (var action in IterationsContainer)
            {
                try
                {
                    if (!action.Initialize())
                    {
                        throw new System.Exception($"Could not initialize action : {action}!");
                    }
                }
                catch (System.Exception e)
                {
                    isAnyErrorHappened = true;
                    Debug.LogError($"Error when initializing action : {action}! Error : {e}");
                    break;
                }
            }

            HasIterationsInitialized = !isAnyErrorHappened;
        }

        private void Start()
        {
            StartActions();
        }

        private void StartActions()
        {
            InitializeActions();
            LoadActions();
            StartCoroutine(MainMenuIterations());
        }

        void LoadActions()
        {
            CurrentIterations ??= new();
            CurrentIterations.Clear();

            foreach (var action in IterationsContainer)
            {
                if (!action.CanStart())
                    continue;

                CurrentIterations.Add(action);
            }
        }

        IEnumerator MainMenuIterations()
        {
            CurrentIterationIndex = -1;

            if (LoadNextIteration())
            {
                while (CurrentIterationIndex < CurrentIterations.Count)
                {
                    CurrentIteration.Update();

                    if (!CurrentIteration.IsFinished())
                    {
                        yield return new WaitForEndOfFrame();
                        continue;
                    }

                    Debug.Log($"Succesfully finished iteration : {CurrentIteration}!");

                    if (!LoadNextIteration())
                        break;
                }
            }

            Debug.Log("Main Menu Iterations are completed!");

            bool LoadNextIteration()
            {
                CurrentIterationIndex++;

                if (CurrentIterationIndex >= CurrentIterations.Count)
                    return false;

                if (CurrentIterationIndex < CurrentIterations.Count)
                    CurrentIteration = CurrentIterations[CurrentIterationIndex];

                if (!CurrentIteration.CanStart())
                    return LoadNextIteration();

                if (!CurrentIteration.OneTimeExecute())
                    return LoadNextIteration();

                Debug.Log($"Succesfully loaded iteration : {CurrentIteration}!");

                return true;
            }
        }
    }
}