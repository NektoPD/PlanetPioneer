using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystemView : MonoBehaviour
{
    [SerializeField] private Button _upgradeWeaponButton;
    [SerializeField] private Button _upgradeBaseButton;
    [SerializeField] private Button _upgradeRocketButton;
    [SerializeField] private UpgradeSystem _upgradeSystem;
    [SerializeField] private TMP_Text _baseUpgradeCost;
    [SerializeField] private TMP_Text _weaponUpgradeCost;
    [SerializeField] private TMP_Text _rocketUpgradeCost;
    [SerializeField] private Sprite _upgradeImage;

    [SerializeField] private BaseUpgrader _baseUpgrade;
    [SerializeField] private RocketBuilder _rocketBuilder;
    
    private WeaponUpgrader _weaponUpgrader;
    private UIUpgradeImageSlot[] _baseUpgradeSlots;
    private UIUpgradeImageSlot[] _weaponUpgradeSlots;
    private UIUpgradeImageSlot[] _rocketUpgradeSlots;

    private CanvasGroup _canvas;

    public event Action OnBaseUpgradeButtonClicked;
    public event Action OnRocketUpgradeButtonClicked;
    public event Action OnWeaponUpgradeButtonClicked;
    public event Action RocketUpgradeButtonEnabled;

    private void Awake()
    {
        _canvas = GetComponentInParent<CanvasGroup>();

        _baseUpgradeSlots = _upgradeBaseButton.GetComponentsInChildren<UIUpgradeImageSlot>();
        _weaponUpgradeSlots = _upgradeWeaponButton.GetComponentsInChildren<UIUpgradeImageSlot>();
        _rocketUpgradeSlots = _upgradeRocketButton.GetComponentsInChildren<UIUpgradeImageSlot>();
    }

    private void Start()
    {
        _upgradeWeaponButton.onClick.AddListener(HandleOnWeaponButtonClick);
        _upgradeBaseButton.onClick.AddListener(HandleOnBaseButtonClick);
        _upgradeRocketButton.onClick.AddListener(HandleOnRocketButtonClick);

        HideUpgradeWindow();
        DisableButton(_upgradeRocketButton);
    }

    private void OnEnable()
    {
        _upgradeSystem.PlayerSteppedIn += ShowUpgradeWindow;
        _upgradeSystem.PlayerSteppedOut += HideUpgradeWindow;

        _upgradeSystem.WeaponUpgradeCostChanged += SetWeaponUpgradeValue;
        _upgradeSystem.BaseUpgradeCostChanged += SetBaseUpgradeValue;
        _upgradeSystem.RocketUpgradeCostChanged += SetRocketUpgradeValue;

        _baseUpgrade.BaseUpgraded += UpgradeBaseSlots;
        _baseUpgrade.BaseFullyUpgraded += HandleBaseFullyUpgraded;

        _rocketBuilder.OnePartUpgraded += UpgradeRocketSlots;
        _rocketBuilder.RocketReady += DiactivateRocketUpgradeButton;

        _weaponUpgrader.WeaponUpgraded += UpgradeWeaponSlots;
        _weaponUpgrader.WeaponFullyUpgraded += DiactivateWeaponUpgradeButton;
    }

    private void OnDisable()
    {
        _upgradeSystem.PlayerSteppedIn -= ShowUpgradeWindow;
        _upgradeSystem.PlayerSteppedOut -= HideUpgradeWindow;

        _upgradeSystem.WeaponUpgradeCostChanged -= SetWeaponUpgradeValue;
        _upgradeSystem.BaseUpgradeCostChanged -= SetBaseUpgradeValue;
        _upgradeSystem.RocketUpgradeCostChanged -= SetRocketUpgradeValue;

        _baseUpgrade.BaseUpgraded -= UpgradeBaseSlots;
        _baseUpgrade.BaseFullyUpgraded -= HandleBaseFullyUpgraded;

        _rocketBuilder.OnePartUpgraded -= UpgradeRocketSlots;
        _rocketBuilder.RocketReady -= DiactivateRocketUpgradeButton;

        _weaponUpgrader.WeaponUpgraded -= UpgradeWeaponSlots;
        _weaponUpgrader.WeaponFullyUpgraded -= DiactivateWeaponUpgradeButton;
    }

    private void HandleOnWeaponButtonClick() => OnWeaponUpgradeButtonClicked?.Invoke();
    private void HandleOnRocketButtonClick() => OnRocketUpgradeButtonClicked?.Invoke();
    private void HandleOnBaseButtonClick() => OnBaseUpgradeButtonClicked?.Invoke();

    private void SetBaseUpgradeValue(int value) => _baseUpgradeCost.text = value.ToString();
    private void SetWeaponUpgradeValue(int value) => _weaponUpgradeCost.text = value.ToString();
    private void SetRocketUpgradeValue(int value) => _rocketUpgradeCost.text = value.ToString();

    private void DiactivateRocketUpgradeButton() => _upgradeRocketButton.enabled = false;
    private void DiactivateWeaponUpgradeButton() => _upgradeWeaponButton.enabled = false;

    private void DisableButton(Button button) => button.gameObject.SetActive(false);
    private void EnableButton(Button button) => button.gameObject.SetActive(true);

    private void UpgradeBaseSlots() => SetUpgradeSlotToActive(_baseUpgradeSlots);
    private void UpgradeWeaponSlots() => SetUpgradeSlotToActive(_weaponUpgradeSlots);
    private void UpgradeRocketSlots() => SetUpgradeSlotToActive(_rocketUpgradeSlots);

    private void ShowUpgradeWindow()
    {
        _canvas.alpha = 1;
        _canvas.blocksRaycasts = true;
    }

    private void HideUpgradeWindow()
    {
        _canvas.alpha = 0;
        _canvas.blocksRaycasts = false;
    }

    private void HandleBaseFullyUpgraded()
    {
        DisableButton(_upgradeBaseButton);
        EnableButton(_upgradeRocketButton);
        RocketUpgradeButtonEnabled?.Invoke();
    }

    private void SetUpgradeSlotToActive(UIUpgradeImageSlot[] slots)
    {
        foreach (var slot in slots)
        {
            if (slot.Activated == false)
            {
                slot.SetUpgradedImage(_upgradeImage);
                break;
            }
        }
    }

    public void SetWeaponUpgrader(WeaponUpgrader weaponUpgrader)
    {
        if(weaponUpgrader == null)
            throw new ArgumentNullException(nameof(weaponUpgrader));

        _weaponUpgrader = weaponUpgrader;
    }
}
