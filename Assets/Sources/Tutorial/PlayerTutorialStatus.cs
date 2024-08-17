using System;
using UnityEngine;

public class PlayerTutorialStatus : MonoBehaviour
{
    private const string TutorialStatusKey = "TutorialStatus";

    [SerializeField] private TutorialWindow _tutorialWindow;

    private bool _isTutorialViewed;

    public bool IsTutorialCompleted => _isTutorialViewed;

    private void OnEnable()
    {
        LoadTutorialStatus();

        _tutorialWindow.TutorialViewed += TutorialCompleted;
    }

    private void OnDisable()
    {
        _tutorialWindow.TutorialViewed -= TutorialCompleted;
    }

    public void TutorialCompleted()
    {
        _isTutorialViewed = true;
        SaveTutorialStatus();
    }

    private void SaveTutorialStatus()
    {
        PlayerPrefs.SetInt(TutorialStatusKey, _isTutorialViewed ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadTutorialStatus()
    {
        if (PlayerPrefs.HasKey(TutorialStatusKey))
        {
            _isTutorialViewed = PlayerPrefs.GetInt(TutorialStatusKey) == 1;
        }
    }
}