using UnityEngine;

public class SoundPlayer : MonoBehaviour
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
