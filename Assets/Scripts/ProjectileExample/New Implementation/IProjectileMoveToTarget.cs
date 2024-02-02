using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileMoveToTarget : IGenericInitializable<IProjectile>
{
    public void Update();
    public void Reset();
}
