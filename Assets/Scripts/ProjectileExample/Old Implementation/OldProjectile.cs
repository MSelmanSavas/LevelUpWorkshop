using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.Projectile.Old
{
    // We need to create another projectile class if we want to change projectile behaviour
    // We can always create it from base class, but changing logic can be hard because of inheritance
    // Instead, we should apply composition!
    public class OldProjectile : MonoBehaviour
    {
        private void Start()
        {
            FindTarget();
            StartCoroutine(MoveToTarget());
        }

        //Changing target find algorith is tedious
        void FindTarget()
        {
            //TODO : Add Find Target Code here
        }


        void DealDamageAtTarget()
        {
            //TODO : Add Damage Target Code here
        }

        IEnumerator MoveToTarget()
        {
            bool hasArrivedToTarget = false;

            while (!hasArrivedToTarget)
            {
                // Move to target
                hasArrivedToTarget = true;
            }

            DealDamageAtTarget();
            yield break;
        }
    }
}