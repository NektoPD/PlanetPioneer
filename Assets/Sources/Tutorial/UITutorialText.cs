using System.Collections;
using TMPro;
using UnityEngine;

public class UITutorialText : MonoBehaviour
{
    [SerializeField] private SoundPlayer _textSound;
    [SerializeField] private TMP_Text _textBox;

    private float _charDisplayInterval = 0.02f;
    private Coroutine _coroutine;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _coroutine = null;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    public void StartDisplayText()
    {
        if (_coroutine != null)
            return;

        _coroutine = StartCoroutine(DisplayTextCoroutine());
    }

    private void SkipButtonPressed()
    {
        if (_coroutine == null)
            return;

        StopCoroutine(_coroutine);

        if (_textSound != null)
        {
            _textSound.StopPlayingSound();
        }

        _textBox.maxVisibleCharacters = _textBox.text.Length;
        _coroutine = null;
    }

    private IEnumerator DisplayTextCoroutine()
    {
        WaitForSeconds interval = new WaitForSeconds(_charDisplayInterval);

        if (_textSound != null)
        {
            _textSound.PlaySound();
        }

        _textBox.maxVisibleCharacters = 0;

        for (int i = 0; i <= _textBox.text.Length; i++)
        {
            _textBox.maxVisibleCharacters = i;
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
        return _textBox.maxVisibleCharacters == _textBox.text.Length;
    }
}