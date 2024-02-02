using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.Projectile.New
{
    public class NewProjectile : MonoBehaviour, IProjectile
    {
        [SerializeReference]
        IProjectileTargetSelector _projectileTargetSelector;

        [SerializeReference]
        IProjectileMoveToTarget _projectileMoveToTarget;

        [SerializeReference]
        IProjectileDamage _projectileDamage;

        private void OnEnable()
        {
            Initialize();
        }

        void Initialize()
        {
            _projectileTargetSelector?.TryInitialize(this);
            _projectileMoveToTarget?.TryInitialize(this);
            _projectileDamage?.TryInitialize(this);
        }

        private void Update()
        {
            _projectileTargetSelector?.Update();
            _projectileMoveToTarget?.Update();
            _projectileDamage?.Update();
        }

        private void OnDisable()
        {
            Reset();
        }

        void Reset()
        {
            _projectileTargetSelector?.Reset();
            _projectileMoveToTarget?.Reset();
            _projectileDamage?.Reset();
        }
    }
}