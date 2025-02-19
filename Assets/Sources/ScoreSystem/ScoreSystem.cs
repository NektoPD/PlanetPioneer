using System;
using UnityEngine;
using Zenject;

public class ScoreSystem : MonoBehaviour
{
    private float _timer;

    public float Timer => _timer;

    [Inject]
    private void Construct(PlanetServicesProvider planetServicesProvider)
    {
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
}