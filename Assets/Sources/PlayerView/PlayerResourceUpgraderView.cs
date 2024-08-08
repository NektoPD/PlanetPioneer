using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceUpgraderView : MonoBehaviour
{
    [SerializeField] private ResourceSlot _crystalResourceSlot;
    [SerializeField] private ResourceSlot _plantResourceSlot;
    [SerializeField] private ResourceSlot _alienArtifactResourceSlot;
    [SerializeField] private WeaponUpgrader _weaponUpgrader;

    private void OnEnable()
    {
        _weaponUpgrader.WeaponSecondLevelUpgraded += EnableCrystalResourceSlot;
        _weaponUpgrader.WeaponThirdLevelUpgraded += EnablePlantResourceSlot;
        _weaponUpgrader.WeaponFullyUpgraded += EnableAlienArtifactResourceSlot;
    }

    private void OnDisable()
    {
        _weaponUpgrader.WeaponFullyUpgraded -= EnableAlienArtifactResourceSlot;
        _weaponUpgrader.WeaponSecondLevelUpgraded -= EnableCrystalResourceSlot;
        _weaponUpgrader.WeaponThirdLevelUpgraded -= EnablePlantResourceSlot;
    }


    private void EnableResourceSlot(ResourceSlot resourceSlot)
    {
        resourceSlot.gameObject.SetActive(true);
    }
    private void EnableCrystalResourceSlot()
    {
        EnableResourceSlot(_crystalResourceSlot);
    }
    
    private void EnablePlantResourceSlot()
    {
        EnableResourceSlot(_plantResourceSlot);
    }

    private void EnableAlienArtifactResourceSlot()
    {
        EnableResourceSlot(_alienArtifactResourceSlot);
    }
}
