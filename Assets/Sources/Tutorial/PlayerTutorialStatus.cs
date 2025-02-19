using System;
using UnityEngine;

public class PlayerTutorialStatus : MonoBehaviour
{
    private const string TutorialStatusKey = "TutorialStatus";

    [SerializeField] private TutorialWindow _tutorialWindow;

    private bool _isTutorialViewed;

    public event Action TutorialCompleted;

    public bool IsTutorialCompleted => _isTutorialViewed;

    private void OnEnable()
    {
        _tutorialWindow.TutorialViewed += CompleteTutorial;
    }

    private void OnDisable()
    {
        _tutorialWindow.TutorialViewed -= CompleteTutorial;
    }

    private void Start()
    {
        LoadTutorialStatus();
    }

    private void CompleteTutorial()
    {
        _isTutorialViewed = true;
        TutorialCompleted?.Invoke();
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

            if (_isTutorialViewed)
            {
                TutorialCompleted?.Invoke();
            }
        }
    }
}