using System;
using UnityEngine;
using TMPro;

public class TutorialWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _skipTutorialText;
    [SerializeField] private TMP_Text _startNextTutorialText;

    private UITutorialText[] _textBoxes;
    private int _currentTutorialWindow;
    private PlayerInput _playerInput;

    public event Action TutorialViewed;

    private void Awake()
    {
        InitializeComponents();
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Skip.performed += ctx => OnSkipButtonPressed();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Player.Skip.performed -= ctx => OnSkipButtonPressed();
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

    private bool IsValidCurrentTutorialWindow()
    {
        return _textBoxes.Length > 0 && _textBoxes[_currentTutorialWindow] != null;
    }

    private void HandleTutorialTextDisplayed()
    {
        _skipTutorialText.enabled = false;
        _startNextTutorialText.enabled = true;
    }

    private void OnSkipButtonPressed()
    {
        if (_textBoxes[_currentTutorialWindow].DisplayedAllText() == false)
            return;
        
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
            _skipTutorialText.enabled = true;
            _startNextTutorialText.enabled = false;
        }
    }
}