using UnityEngine;
using TMPro;

public class TutorialWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _skipTutorialText;
    [SerializeField] private TMP_Text _startNextTutorialText;

    private UITutorailText[] _textBoxes;
    private int _currentTutorialWindow;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        DeactivateAllTextBoxes();
        ActivateCurrentTextBox();
        _startNextTutorialText.enabled = false;
    }

    private void Update()
    {
        if (!IsValidCurrentTutorialWindow())
        {
            return;
        }

        if (_textBoxes[_currentTutorialWindow].DisplayedAllText())
        {
            HandleTutorialTextDisplayed();
        }
    }

    private void InitializeComponents()
    {
        _textBoxes = GetComponentsInChildren<UITutorailText>();
        _currentTutorialWindow = 0;
    }

    private void DeactivateAllTextBoxes()
    {
        foreach (var box in _textBoxes)
        {
            box.gameObject.SetActive(false);
        }
    }

    private void ActivateCurrentTextBox()
    {
        if (_textBoxes.Length > 0)
        {
            _textBoxes[_currentTutorialWindow].gameObject.SetActive(true);
        }
    }

    private bool IsValidCurrentTutorialWindow()
    {
        return _textBoxes.Length > 0 && _textBoxes[_currentTutorialWindow] != null;
    }

    private void HandleTutorialTextDisplayed()
    {
        _skipTutorialText.enabled = false;
        _startNextTutorialText.enabled = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProceedToNextTutorialWindow();
        }
    }

    private void ProceedToNextTutorialWindow()
    {
        _textBoxes[_currentTutorialWindow].gameObject.SetActive(false);
        _currentTutorialWindow++;

        if (_currentTutorialWindow >= _textBoxes.Length)
        {
            gameObject.SetActive(false);
        }
        else
        {
            ActivateCurrentTextBox();
            _skipTutorialText.enabled = true;
            _startNextTutorialText.enabled = false;
        }
    }
}
