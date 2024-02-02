using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileDamage : IGenericInitializable<IProjectile>
{
    public void Update();
    public void Reset();
}
