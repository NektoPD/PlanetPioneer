using System;

public interface IPlayerUpgrader
{
    public event Action UpgradedBag;
    public event Action UpgradedGatherSpeed;
    public event Action UpgradedPlayerMovingSpeed;
    public event Action UpgradedGatherRadius;
}