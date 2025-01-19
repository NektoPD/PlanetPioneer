using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MainGameSettingsMenu : MonoBehaviour
{
    private const string PlayerSoundPreferncesParameterName = "SoundVolume";

    [SerializeField] private Button _settingsButton;
    [SerializeField] private Canvas _settingsCanvas;
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _effectsVolumeSlider;
    [SerializeField] private Slider _backgroundVolumeSlider;
    [SerializeField] private Toggle _musicSwitch;
    [SerializeField] private SoundController _soundController;
    [SerializeField] private Button _leaderBoardButton;
    [SerializeField] private PlayerTutorialStatus _playerTutorialStatus;
    [SerializeField] private GameObject _leaderboard;

    private void OnEnable()
    {
        _settingsButton.onClick.AddListener(ProcessSettingsButtonClick);
        _backButton.onClick.AddListener(ProcessBackButtonClick);
        _effectsVolumeSlider.onValueChanged.AddListener(ProcessEffectVolumeSliderValueChanged);
        _backgroundVolumeSlider.onValueChanged.AddListener(ProcessBackgroundVolumeSliderValueChanged);
        _musicSwitch.onValueChanged.AddListener(DisableMusic);
        _leaderBoardButton.onClick.AddListener(ProcessLeaderBoardButtonClick);
        _playerTutorialStatus.TutorialCompleted += EnableSettingsButton;
    }

    private void OnDisable()
    {
        _settingsButton.onClick.RemoveListener(ProcessSettingsButtonClick);
        _backButton.onClick.RemoveListener(ProcessBackButtonClick);
        _effectsVolumeSlider.onValueChanged.RemoveListener(ProcessEffectVolumeSliderValueChanged);
        _backgroundVolumeSlider.onValueChanged.RemoveListener(ProcessBackgroundVolumeSliderValueChanged);
        _musicSwitch.onValueChanged.RemoveListener(DisableMusic);
        _leaderBoardButton.onClick.RemoveListener(ProcessLeaderBoardButtonClick);
        _playerTutorialStatus.TutorialCompleted -= EnableSettingsButton;
    }

    private void Start()
    {
        _settingsCanvas.enabled = false;

        if (!_playerTutorialStatus.IsTutorialCompleted)
            _settingsButton.interactable = false;

        _effectsVolumeSlider.value = _effectsVolumeSlider.maxValue;
        _backgroundVolumeSlider.value = _backgroundVolumeSlider.maxValue;

        if (PlayerPrefs.HasKey(PlayerSoundPreferncesParameterName))
        {
            ProcessEffectVolumeSliderValueChanged(PlayerPrefs.GetFloat(PlayerSoundPreferncesParameterName));
        }

        ProcessLeaderBoardButtonClick();
    }

    private void ProcessLeaderBoardButtonClick()
    {
        ToggleLeaderboard(!_leaderboard.activeSelf);
    }

    private void EnableSettingsButton()
    {
        _settingsButton.interactable = true;
    }

    private void ProcessSettingsButtonClick()
    {
        if (_settingsCanvas.enabled)
            return;

        _settingsButton.enabled = false;
        _settingsCanvas.enabled = true;
        YandexGame.GameplayStop();
    }

    private void ProcessBackButtonClick()
    {
        if (_settingsCanvas.enabled == false)
            return;

        _settingsButton.enabled = true;
        _settingsCanvas.enabled = false;
        YandexGame.GameplayStart();
    }

    private void ProcessEffectVolumeSliderValueChanged(float value)
    {
        if (_soundController != null)
            _soundController.ChangeEffectsVolume(value);
        else
            SaveSoundPreferences(value);
    }

    private void ProcessBackgroundVolumeSliderValueChanged(float value)
    {
        if (_soundController != null)
            _soundController.ChangeBackgroundVolume(value);
        else
            SaveSoundPreferences(value);
    }

    private void DisableMusic(bool enabled)
    {
        _soundController.ToggleMusic(enabled);
    }

    private void SaveSoundPreferences(float value)
    {
        PlayerPrefs.SetFloat(PlayerSoundPreferncesParameterName, value);
    }

    private void ToggleLeaderboard(bool status)
    {
        _leaderboard.SetActive(status);
    }
}