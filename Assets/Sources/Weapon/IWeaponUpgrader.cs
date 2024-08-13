using System;

public interface IWeaponUpgrader
{
    public event Action WeaponUpgraded;
    public event Action WeaponFullyUpgraded;
    public event Action WeaponSecondLevelUpgraded;
    public event Action WeaponThirdLevelUpgraded;

    public int StartLevel { get; }
    public int SecondLevel { get; }
    public int ThirdLevel { get; }
    public int EndLevel { get; }
    public int CurrentLevel { get; }

    public void SetCurrentLevel(int level);
}