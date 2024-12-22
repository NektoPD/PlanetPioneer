using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    private const string EffectsVolumeMixerName = "Effects";
    private const string BackgroundVolumeMixerName = "Background";
    private const float MinVolume = -80f;
    private const float MaxVolume = 0f;
    
    [SerializeField] private AudioMixerGroup _audioMixer;

    public float CurrentVolume => _audioMixer.audioMixer.GetFloat(EffectsVolumeMixerName, out float value) ? value : MaxVolume;
    
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
        if (enabled)
            _audioMixer.audioMixer.SetFloat(BackgroundVolumeMixerName, 0f);
        else
            _audioMixer.audioMixer.SetFloat(BackgroundVolumeMixerName, -80f);
    }
}
