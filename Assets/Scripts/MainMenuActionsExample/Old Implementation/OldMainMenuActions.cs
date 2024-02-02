using System.Collections;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.MainMenu.Old
{
    /// Changing things here is prone to git merge conflicts.
    /// Two person should not work on the same file unless it is crucial
    public class OldMainMenuActions : MonoBehaviour
    {
        [SerializeReference]
        System1 _system1;

        [SerializeReference]
        System2 _system2;

        [SerializeReference]
        System3 _system3;

        private void Start()
        {
            FindSystems();
        }

        void OnReturnToMainMenu()
        {
            StartCoroutine(Actions());
        }

        IEnumerator Actions()
        {
            OpenRewards();

            yield return new WaitForSeconds(2);
            ChangeThings();

            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Mouse0));

            DoStuff();
            Backflip();

            yield return new WaitForEndOfFrame();

            FinalizeActions();

            yield break;
        }

        void FindSystems()
        {
            _system1 = new System1();
            _system2 = new System2();
            _system3 = new System3();
        }

        void OpenRewards()
        {
            _system1.Stuff1();
        }

        void ChangeThings()
        {
            _system2.Stuff2();
        }

        void DoStuff()
        {
            _system3.Stuff3();
        }

        void Backflip() { }
        void FinalizeActions() { }
    }
}

