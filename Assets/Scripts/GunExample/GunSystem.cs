using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.Gun
{
    public class GunSystem : MonoBehaviour
    {
        [SerializeField]
        public IGunLoader GunLoader { get; private set; }

        void LoadGun(ScriptableGunData scriptableGunData)
        {
            GunLoader.TryLoadGun(scriptableGunData);
        }
    }
}
