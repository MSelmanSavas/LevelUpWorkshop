using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.Gun
{
    [CreateAssetMenu(fileName = "NewGunData", menuName = "Create/Gun/Create Gun Data")]
    public class ScriptableGunData : ScriptableObject
    {
        [field: SerializeField]
        public GameObject GunPrefab { get; private set; }

        [field: SerializeField]
        public TypeReferenceInheritedFrom<IGunAim> AimType { get; private set; }

          [field: SerializeField]
        public TypeReferenceInheritedFrom<IGunAim> LoaderType { get; private set; }

          [field: SerializeField]
        public TypeReferenceInheritedFrom<IGunAim> ReloadType { get; private set; }

          [field: SerializeField]
        public TypeReferenceInheritedFrom<IGunAim> TriggerType { get; private set; }
    }
}