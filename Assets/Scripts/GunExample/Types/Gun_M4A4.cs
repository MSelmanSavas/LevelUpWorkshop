using LevelUp.UZCY.CodeAbstraction.Examples.Gun;
using UnityEngine;

public class Gun_M4A4 : MonoBehaviour, IGun
{
    [SerializeReference]
    IGunAim _gunAim;

    [SerializeReference]
    IGunTrigger _gunTrigger;

    [SerializeReference]
    IGunReload _gunReload;

    public IGunAim GetGunAim() => _gunAim;
    public IGunReload GetGunReload() => _gunReload;
    public IGunTrigger GetGunTrigger() => _gunTrigger;

}
