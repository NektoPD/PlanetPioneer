using UnityEngine;

public class PlayerTutorialStatus : MonoBehaviour
{
    private const string TutorialStatusKey = "TutorialStatus";

    [SerializeField] private TutorialWindow _tutorialWindow;

    private bool _isTutorialViewed = false;

    public bool IsTutorialCompleted => _isTutorialViewed;
    
    private void OnEnable()
    {
        if (LoadTutorialStatus() == false)
            SetDefaultTutorialStatus();

        _tutorialWindow.TutorialViewed += TutorialCompleted;
    }

    private void OnDisable()
    {
        if (_tutorialWindow != null)
        {
            _tutorialWindow.TutorialViewed -= TutorialCompleted;
        }
    }

    public void TutorialCompleted()
    {
        _isTutorialViewed = true;
        SaveTutorialStatus();

        if (_tutorialWindow != null)
        {
            _tutorialWindow.TutorialViewed -= TutorialCompleted;
        }
    }

    public void SetDefaultTutorialStatus()
    {
        _isTutorialViewed = false;
        SaveTutorialStatus();
    }

    private void SaveTutorialStatus()
    {
        PlayerPrefs.SetInt(TutorialStatusKey, _isTutorialViewed ? 1 : 0);
        PlayerPrefs.Save();
    }

    private bool LoadTutorialStatus()
    {
        if (PlayerPrefs.HasKey(TutorialStatusKey))
        {
            _isTutorialViewed = PlayerPrefs.GetInt(TutorialStatusKey, 0) == 1;
            return true;
        }

        return false;
    }
}