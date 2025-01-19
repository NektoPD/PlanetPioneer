using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPopUpWindowShower : MonoBehaviour
{
    private const string PopUpAnimationName = "PopUpAnimation";

    [SerializeField] private TMP_Text _popupText;

    private readonly float _delayBetweenAnimations = 3;
    private Animator _animator;

    private CanvasGroup _canvas;
    private Queue<(string message, Action onMessageShown)> _popupQueue = new Queue<(string, Action)>();
    private IEnumerator _popupCoroutine;

    private void Awake()
    {
        _canvas = GetComponentInChildren<CanvasGroup>();
        _animator = _canvas.GetComponent<Animator>();
        _canvas.alpha = 0;
        _popupCoroutine = ProcessMessageQueue();
    }

    public void AddMessageToQueue(string message, Action onMessageShown = null)
    {
        _popupQueue.Enqueue((Lean.Localization.LeanLocalization.GetTranslationText(message), onMessageShown));

        _popupCoroutine = ProcessMessageQueue();
        StartCoroutine(_popupCoroutine);
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
            var (message, onMessageShown) = _popupQueue.Dequeue();
            ShowPopUp(message);

            yield return delay;

            _canvas.alpha = 0;
            onMessageShown?.Invoke();
        }
        
        StopCoroutine();
    }

    private void StopCoroutine()
    {
        if (_popupCoroutine != null)
        {
            StopCoroutine(_popupCoroutine);
            _popupCoroutine = null;
        }
    }
}