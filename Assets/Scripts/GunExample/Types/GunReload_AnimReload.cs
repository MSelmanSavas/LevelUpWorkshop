using LevelUp.UZCY.CodeAbstraction.Examples.Gun;

public class GunReload_AnimReload : IGunReload
{
    public bool TryInitialize(IGun data)
    {
        return true;
    }
}
