using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelUp.UZCY.CodeAbstraction.Examples.SaveLoad
{
    public interface ISaveable
    {
        SaveData SaveData();
        void LoadData(SaveData data);
    }
}