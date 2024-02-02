using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.Gun
{
    public interface IGunLoader
    {
        bool TryLoadGun(ScriptableGunData gunData);
    }
}
