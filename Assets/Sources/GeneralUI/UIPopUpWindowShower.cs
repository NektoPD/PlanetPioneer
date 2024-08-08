using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPopUpWindowShower : MonoBehaviour
{
    private const string PopUpAnimationName = "PopUpAnimation";

    [SerializeField] private TMP_Text _popupText;

    private float _delayBetweenAnimations = 3;
    private Animator _animator;

    private CanvasGroup _canvas;
    private Queue<string> _popupQueue = new Queue<string>();
    private Coroutine _popupCoroutine;

    private void Awake()
    {
        _canvas = GetComponentInChildren<CanvasGroup>();
        _animator = _canvas.GetComponent<Animator>();
        _canvas.alpha = 0;
    }

    public void AddMessageToQueue(string message)
    {
        _popupQueue.Enqueue(message);

        if(_popupCoroutine != null)
            return;

        _popupCoroutine = StartCoroutine(ProcessMessageQueue());
    }

    private void ShowPopUp(string message)
    {
        _canvas.alpha = 1;
        _popupText.text = message;
        _animator.Play(PopUpAnimationName);
    }

    private IEnumerator ProcessMessageQueue()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayBetweenAnimations);

        while (_popupQueue.Count > 0)
        {
            string message = _popupQueue.Dequeue();
            ShowPopUp(message);

            yield return delay;

            _canvas.alpha = 0;
        }

        _popupCoroutine = null;
    }
}
