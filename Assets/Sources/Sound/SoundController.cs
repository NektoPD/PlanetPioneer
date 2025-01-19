using UnityEngine;
using UnityEngine.Audio;
using YG;

public class SoundController : MonoBehaviour
{
    private const string EffectsVolumeMixerName = "Effects";
    private const string BackgroundVolumeMixerName = "Background";
    private const float MinVolume = -80f;
    private const float MaxVolume = -20f;

    [SerializeField] private AudioMixerGroup _audioMixer;
    [SerializeField] private AudioSource _backgroundMusic;

    public float CurrentVolume =>
        _audioMixer.audioMixer.GetFloat(EffectsVolumeMixerName, out float value) ? value : MaxVolume;

    private void OnEnable()
    {
        Application.focusChanged += ToggleAllSounds;
        YandexGame.GameplayStatusChanged += (() => ToggleAllSounds(YandexGame.isGamePlaying));
        YandexGame.OpenFullAdEvent += () => ToggleAllSounds(false);
        YandexGame.CloseFullAdEvent += () => ToggleAllSounds(true);
    }

    private void OnDisable()
    {
        Application.focusChanged -= ToggleAllSounds;
        YandexGame.GameplayStatusChanged -= (() => ToggleAllSounds(YandexGame.isGamePlaying));
        YandexGame.OpenFullAdEvent -= () => ToggleAllSounds(false);
        YandexGame.CloseFullAdEvent -= () => ToggleAllSounds(true);
    }

    private void Update()
    {
        if(!YandexGame.isGamePlaying)
            ToggleAllSounds(false);
    }

    public void ChangeEffectsVolume(float volume)
    {
        _audioMixer.audioMixer.SetFloat(EffectsVolumeMixerName, Mathf.Lerp(MinVolume, MaxVolume, volume));
    }

    public void ChangeBackgroundVolume(float volume)
    {
        _audioMixer.audioMixer.SetFloat(BackgroundVolumeMixerName, Mathf.Lerp(MinVolume, MaxVolume, volume));
    }

    public void ToggleMusic(bool enabled)
    {
        _audioMixer.audioMixer.SetFloat(BackgroundVolumeMixerName, enabled ? MaxVolume : MinVolume);
    }
    
    private void ToggleAllSounds(bool hasFocus)
    {
        if (!hasFocus)
        {
            AudioListener.volume = 0;
            AudioListener.pause = true;
            return;
        }
        
        AudioListener.volume = 1;
        AudioListener.pause = false;
    }
}