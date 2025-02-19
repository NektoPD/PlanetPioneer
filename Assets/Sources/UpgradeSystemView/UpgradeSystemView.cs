using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystemView : MonoBehaviour
{
    private const int ActiveCanvasValue = 1;
    private const int DeactivatedCanvasValue = 0;

    [SerializeField] private Button _upgradeWeaponButton;
    [SerializeField] private Button _upgradeBaseButton;
    [SerializeField] private Button _upgradeRocketButton;
    [SerializeField] private Button _pushToWatchAdButton;
    [SerializeField] private UpgradeSystem _upgradeSystem;
    [SerializeField] private TMP_Text _baseUpgradeCost;
    [SerializeField] private TMP_Text _baseUpgradeText;
    [SerializeField] private TMP_Text _weaponUpgradeCost;
    [SerializeField] private TMP_Text _rocketUpgradeCost;
    [SerializeField] private TMP_Text _rocketUpgradeText;
    [SerializeField] private Sprite _upgradeImage;
    [SerializeField] private BaseUpgrader _baseUpgrade;
    [SerializeField] private RocketBuilder _rocketBuilder;
    [SerializeField] private UIUpgradeImageSlot[] _baseUpgradeSlots;
    [SerializeField] private UIUpgradeImageSlot[] _weaponUpgradeSlots;
    [SerializeField] private UIUpgradeImageSlot[] _rocketUpgradeSlots;

    private WeaponUpgrader _weaponUpgrader;

    private CanvasGroup _canvas;

    public event Action OnBaseUpgradeButtonClicked;
    public event Action OnRocketUpgradeButtonClicked;
    public event Action OnWeaponUpgradeButtonClicked;
    public event Action RocketUpgradeButtonEnabled;
    public event Action WatchAdButtonClicked;

    private void Awake()
    {
        _canvas = GetComponentInParent<CanvasGroup>();
    }

    private void Start()
    {
        _upgradeWeaponButton.onClick.AddListener(HandleOnWeaponButtonClick);
        _upgradeBaseButton.onClick.AddListener(HandleOnBaseButtonClick);
        _upgradeRocketButton.onClick.AddListener(HandleOnRocketButtonClick);
        _pushToWatchAdButton.onClick.AddListener(HandleWatchAddButtonPushed);

        HideUpgradeWindow();
        _upgradeRocketButton.gameObject.SetActive(false);
        _rocketUpgradeCost.enabled = false;
        _rocketUpgradeText.enabled = false;

        foreach (var element in _rocketUpgradeSlots)
        {
            element.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _upgradeSystem.PlayerSteppedIn += ShowUpgradeWindow;
        _upgradeSystem.PlayerSteppedOut += HideUpgradeWindow;

        _upgradeSystem.WeaponUpgradeCostChanged += SetWeaponUpgradeValue;
        _upgradeSystem.BaseUpgradeCostChanged += SetBaseUpgradeValue;
        _upgradeSystem.RocketUpgradeCostChanged += SetRocketUpgradeValue;

        _baseUpgrade.BaseUpgraded += UpgradeBaseSlots;
        _baseUpgrade.LoadedBaseUpgrades += UpgradeBaseSlots;
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
        _baseUpgrade.LoadedBaseUpgrades -= UpgradeBaseSlots;
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

    private void DisableButton(Button button) => button.interactable = false;

    private void UpgradeBaseSlots() => SetUpgradeSlotToActive(_baseUpgradeSlots);

    private void UpgradeWeaponSlots() => SetUpgradeSlotToActive(_weaponUpgradeSlots);

    public void UpgradeRocketSlots() => SetUpgradeSlotToActive(_rocketUpgradeSlots);

    private void ShowUpgradeWindow()
    {
        _canvas.alpha = ActiveCanvasValue;
        _canvas.blocksRaycasts = true;
    }

    private void HideUpgradeWindow()
    {
        _canvas.alpha = DeactivatedCanvasValue;
        _canvas.blocksRaycasts = false;
    }

    private void HandleWatchAddButtonPushed()
    {
        WatchAdButtonClicked?.Invoke();
    }

    private void HandleBaseFullyUpgraded()
    {
        DisableButton(_upgradeBaseButton);
        _baseUpgradeCost.enabled = false;
        _baseUpgradeText.enabled = false;

        _upgradeRocketButton.gameObject.SetActive(true);
        _rocketUpgradeCost.enabled = true;
        _rocketUpgradeText.enabled = true;

        RocketUpgradeButtonEnabled?.Invoke();

        foreach (var element in _rocketUpgradeSlots)
        {
            element.gameObject.SetActive(true);
        }
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
        if (weaponUpgrader == null)
            throw new ArgumentNullException(nameof(weaponUpgrader));

        _weaponUpgrader = weaponUpgrader;
    }
}