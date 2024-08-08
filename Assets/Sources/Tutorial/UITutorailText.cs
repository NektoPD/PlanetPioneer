using System.Collections;
using TMPro;
using UnityEngine;

public class UITutorailText : MonoBehaviour
{
    [SerializeField] private SoundController _textSound;

    private TMP_Text _textBox;
    private string _text;
    private float _charDisplayInterval = 0.02f;
    private Coroutine _coroutine;

    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
        _text = _textBox.text;
        _textBox.text = "";

    }

    private void Update()
    {
        if(_coroutine == null)
            _coroutine = StartCoroutine(DisplayTextCoroutine());

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _textBox.text = _text;
            _textSound.StopPlayingSound();

            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator DisplayTextCoroutine()
    {
        WaitForSeconds interval = new WaitForSeconds(_charDisplayInterval);
        _textSound.PlaySound();

        foreach (char c in _text)
        {
            _textBox.text += c; 
            yield return interval; 
        }

        _textSound.StopPlayingSound();
    }

    public bool DisplayedAllText()
    {
        return _textBox.text == _text;
    }
}
