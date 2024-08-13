using System;
using UnityEngine;

public class WeaponLevelChecker : MonoBehaviour,IWeaponLevelChecker
{
    private IWeaponUpgrader _weaponUpgrader;

    public bool IsWeaponLevelSufficient(Resource resource)
    {
        switch (resource)
        {
            case Iron:
                return _weaponUpgrader.CurrentLevel >= _weaponUpgrader.StartLevel;

            case Crystal:
                return _weaponUpgrader.CurrentLevel >= _weaponUpgrader.SecondLevel;

            case Plant:
                return _weaponUpgrader.CurrentLevel >= _weaponUpgrader.ThirdLevel;

            case AlienArtifact:
                return _weaponUpgrader.CurrentLevel >= _weaponUpgrader.EndLevel;
        }

        return true;
    }

    public void SetWeaponUpgrader(IWeaponUpgrader weaponUpgrader)
    {
        if(weaponUpgrader == null)
            throw new ArgumentNullException(nameof(weaponUpgrader));

        _weaponUpgrader = weaponUpgrader;
    }
}
