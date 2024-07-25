using System;

public interface IUpgradeSystem
{
    public event Action WeaponUpgraded;
    public event Action RocketUpgraded;
    public event Action BaseUpgraded;
}
