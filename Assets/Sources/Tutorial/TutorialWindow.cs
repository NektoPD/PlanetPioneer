using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
{
    [SerializeField] private Button _startNextTutorialText;
    [SerializeField] private PlayerTutorialStatus _playerTutorialStatus;

    private UITutorialText[] _textBoxes;
    private int _currentTutorialWindow;
    private PlayerInput _playerInput;

    public event Action TutorialViewed;

    private void Awake()
    {
        InitializeComponents();
        DeactivateAllTextBoxes();
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _startNextTutorialText.onClick.AddListener(OnSkipButtonPressed);
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _startNextTutorialText.onClick.RemoveListener(OnSkipButtonPressed);
    }

    private void Start()
    {
        if (!_playerTutorialStatus.IsTutorialCompleted)
        {
            ActivateCurrentTextBox();
            _startNextTutorialText.gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void InitializeComponents()
    {
        _textBoxes = GetComponentsInChildren<UITutorialText>();
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
            _textBoxes[_currentTutorialWindow].StartDisplayText();
        }
    }

    private void OnSkipButtonPressed()
    {
        ProceedToNextTutorialWindow();
    }

    private void ProceedToNextTutorialWindow()
    {
        _textBoxes[_currentTutorialWindow].gameObject.SetActive(false);
        _currentTutorialWindow++;

        if (_currentTutorialWindow >= _textBoxes.Length)
        {
            TutorialViewed?.Invoke();
            gameObject.SetActive(false);
        }
        else
        {
            ActivateCurrentTextBox();
        }
    }
}