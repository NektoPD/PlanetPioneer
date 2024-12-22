using System;
using UnityEngine;
using Zenject;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private YandexLeaderboard _leaderboard;
    
    private RocketBuilder _rocketBuilder;
    private float _timer;

    public float Timer => _timer;

    [Inject]
    private void Construct(PlanetServicesProvider planetServicesProvider)
    {
        _rocketBuilder = planetServicesProvider.RocketBuilder;
        _rocketBuilder.RocketReady += SavePlayerScore;
    }

    private void OnDisable()
    {
        _rocketBuilder.RocketReady -= SavePlayerScore;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    public void SetTimer(float value)
    {
        if (value < 0)
            throw new InvalidOperationException(nameof(value));

        _timer = value;
    }

    private void SavePlayerScore()
    {
        _leaderboard.SetPlayerScore(_timer);
    }
    
}
