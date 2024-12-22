using System;
using UnityEngine;
using Agava.WebUtility;

public class FocusChecker : MonoBehaviour
{
    [SerializeField] private SoundController _soundController;

#if !UNITY_EDITOR
    private void OnEnable()
    {
        Application.focusChanged += OnInBackgroundChangeApp;
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
    }

    private void OnDisable()
    {
        Application.focusChanged -= OnInBackgroundChangeApp;
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
    }

    private void OnInBackgroundChangeApp(bool inApp)
    {
        MuteAudio(!inApp);
        PauseGame(!inApp); 
    }

    private void OnInBackgroundChangeWeb(bool isBackground)
    {
        MuteAudio(isBackground);
        PauseGame(isBackground);
    }

    private void PauseGame(bool value) 
    {
        Time.timeScale = value ? 0 : 1; 
    }

    private void MuteAudio(bool value)
    {
        _soundController.ToggleMusic(value);
    }
#endif
}