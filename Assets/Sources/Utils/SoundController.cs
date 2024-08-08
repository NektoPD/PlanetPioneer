using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _sound;

    public void PlaySound()
    {
        _sound.Play();
    }

    public void StopPlayingSound()
    {
        _sound.Stop();
    }
}
