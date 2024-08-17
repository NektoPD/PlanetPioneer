using UnityEngine;
using UnityEngine.UI;

public class MainGameSettingsMenu : MonoBehaviour
{
    private const string PlayerSoundPreferncesParameterName = "SoundVolume";
    
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Canvas _settingsCanvas;
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private SoundController _soundController;

    private void Start()
    {
        _settingsCanvas.enabled = false;
        _settingsButton.onClick.AddListener(ProcessSettingsButtonClick);
        _backButton.onClick.AddListener(ProcessBackButtonClick);
        _volumeSlider.onValueChanged.AddListener(ProcessVolumeSliderValueChanged);
        _volumeSlider.value = _volumeSlider.maxValue;

        if (PlayerPrefs.HasKey(PlayerSoundPreferncesParameterName))
        {
            ProcessVolumeSliderValueChanged(PlayerPrefs.GetFloat(PlayerSoundPreferncesParameterName));
        }
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

    private void ProcessVolumeSliderValueChanged(float value)
    {
        if (_soundController != null)
            _soundController.ChangeVolume(value);
        else
            SaveSoundPreferences(value);
    }

    private void SaveSoundPreferences(float value)
    {
        PlayerPrefs.SetFloat(PlayerSoundPreferncesParameterName, value);
    }
}