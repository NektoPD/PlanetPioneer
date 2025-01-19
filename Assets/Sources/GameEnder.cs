using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameEnder : MonoBehaviour
{
    [SerializeField] private RocketBuilder _rocketBuilder;
    [SerializeField] private SoundPlayer _endGameSound;
    [SerializeField] private GameObject _endGamePlane;
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private ScoreSystem _scoreSystem;

    public event Action RestartGame;

    private void OnEnable()
    {
        _rocketBuilder.RocketReady += EndGame;
        _playAgainButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _rocketBuilder.RocketReady -= EndGame;
        _playAgainButton.onClick.RemoveListener(OnButtonClicked);
    }

    private void Start()
    {
        _endGamePlane.SetActive(false);
    }

    private void EndGame()
    {
        _endGameSound.PlaySound();
        _endGamePlane.SetActive(true);
        YandexGame.NewLBScoreTimeConvert("Leaderboard", _scoreSystem.Timer);
    }

    private void OnButtonClicked()
    {
        RestartGame?.Invoke();
        _endGamePlane.SetActive(false);
    }
}