using System.Collections;
using TMPro;
using UnityEngine;

public class UITutorialText : MonoBehaviour
{
    [SerializeField] private SoundPlayer _textSound;

    private TMP_Text _textBox;
    private string _text;
    private float _charDisplayInterval = 0.02f;
    private Coroutine _coroutine;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
        _text = _textBox.text;
        _textBox.text = "";
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Skip.performed += ctx => SkipButtonPressed();
    }

    private void OnDisable()
    {
        _playerInput.Player.Skip.performed -= ctx => SkipButtonPressed();
        _playerInput.Disable();
    }

    public void StartDisplayText()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(DisplayTextCoroutine());
    }

    private void SkipButtonPressed()
    {
        if (_coroutine == null)
            return;

        StopCoroutine(_coroutine);
        _coroutine = null;

        _textBox.text = _text;
        
        if (_textSound != null)
        {
            _textSound.StopPlayingSound();
        }
    }

    private IEnumerator DisplayTextCoroutine()
    {
        WaitForSeconds interval = new WaitForSeconds(_charDisplayInterval);

        if (_textSound != null)
        {
            _textSound.PlaySound();
        }

        foreach (char c in _text)
        {
            _textBox.text += c;
            yield return interval;
        }

        if (_textSound != null)
        {
            _textSound.StopPlayingSound();
        }

        _coroutine = null;
    }

    public bool DisplayedAllText()
    {
        return _textBox.text == _text;
    }
}