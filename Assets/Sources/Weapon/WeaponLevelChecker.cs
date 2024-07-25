using System;
using UnityEngine;

public class WeaponLevelChecker : MonoBehaviour
{
    private WeaponUpgrader _upgrader;

    public bool IsWeaponLevelSufficient(Resource resource)
    {
        switch (resource)
        {
            case Iron:
                return _upgrader.CurrentLevel >= _upgrader.StartLevel;

            case Crystal:
                return _upgrader.CurrentLevel >= _upgrader.SecondLevel;

            case Plant:
                return _upgrader.CurrentLevel >= _upgrader.ThirdLevel;

            case AlienArtifact:
                return _upgrader.CurrentLevel >= _upgrader.EndLevel;
        }

        return true;
    }

    public void SetWeaponUpgrader(WeaponUpgrader upgrader)
    {
        if(upgrader == null)
            throw new ArgumentNullException(nameof(upgrader));

        _upgrader = upgrader;
    }
}
