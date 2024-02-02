using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.Gun
{
    public interface IGun
    {
        public IGunAim GetGunAim();
        public IGunReload GetGunReload();
        public IGunTrigger GetGunTrigger();
    }
}
