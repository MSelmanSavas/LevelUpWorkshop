using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.Gun
{
    public class GunAim_Ironsight : IGunAim
    {
        public bool TryInitialize(IGun data)
        {
            return true;
        }
    }
}