using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISliderShower : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private float _startValue = 0;
    private float _endValue = 1;
    private Coroutine _coroutine;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        _slider.value = _startValue;
        _canvasGroup.alpha = 0;
    }

    public void ActivateSlider(float speed)
    {
        if (_coroutine != null)
            return;

        _canvasGroup.alpha = 1;
        _coroutine = StartCoroutine(ChangeSliderValue(speed));
    }

    private IEnumerator ChangeSliderValue(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _slider.value = Mathf.Lerp(_startValue, _endValue, elapsedTime / duration);
            yield return null;
        }

        _slider.value = _endValue;

        _canvasGroup.alpha = 0;
        _slider.value = _startValue;
        _coroutine = null;
    }
}
