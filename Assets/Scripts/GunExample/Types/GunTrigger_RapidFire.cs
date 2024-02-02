using LevelUp.UZCY.CodeAbstraction.Examples.Gun;

public class GunTrigger_RapidFire : IGunTrigger
{
    public bool TryInitialize(IGun data)
    {
        return true;
    }
}
