using System;
using UnityEngine;
using UnityEngine.UI;

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

    public event Action LeaderboardButtonClicked;

    private void Start()
    {
        _settingsCanvas.enabled = false;
        _settingsButton.onClick.AddListener(ProcessSettingsButtonClick);
        _backButton.onClick.AddListener(ProcessBackButtonClick);
        _effectsVolumeSlider.onValueChanged.AddListener(ProcessEffectVolumeSliderValueChanged);
        _effectsVolumeSlider.value = _effectsVolumeSlider.maxValue;
        _backgroundVolumeSlider.onValueChanged.AddListener(ProcessBackgroundVolumeSliderValueChanged);
        _backgroundVolumeSlider.value = _backgroundVolumeSlider.maxValue;
        _musicSwitch.onValueChanged.AddListener(DisableMusic);
        _leaderBoardButton.onClick.AddListener(ProcessLeaderBoardButtonClick);

        if (PlayerPrefs.HasKey(PlayerSoundPreferncesParameterName))
        {
            ProcessEffectVolumeSliderValueChanged(PlayerPrefs.GetFloat(PlayerSoundPreferncesParameterName));
        }
    }

    private void ProcessLeaderBoardButtonClick()
    {
        LeaderboardButtonClicked?.Invoke();
    }

    private void ProcessSettingsButtonClick()
    {
        if(_settingsCanvas.enabled)
            return;

        _settingsButton.enabled = false;
        _settingsCanvas.enabled = true;
    }

    private void ProcessBackButtonClick()
    {
        if(_settingsCanvas.enabled == false)
            return;

        _settingsButton.enabled = true;
        _settingsCanvas.enabled = false;
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
}