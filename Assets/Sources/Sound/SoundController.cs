using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    private const string MasterVolumeMixerName = "MasterVolume";
    private const float MinVolume = -80f;
    private const float MaxVolume = 0f;
    
    [SerializeField] private AudioMixerGroup _audioMixer;

    public float CurrentVolume => _audioMixer.audioMixer.GetFloat(MasterVolumeMixerName, out float value) ? value : MaxVolume;
    
    public void ChangeVolume(float volume)
    {
        _audioMixer.audioMixer.SetFloat(MasterVolumeMixerName, Mathf.Lerp(MinVolume, MaxVolume, volume));
    }
}
