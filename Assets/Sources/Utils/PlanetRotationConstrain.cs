using System;
using UnityEngine;
using Zenject;

public class PlanetRotationConstrain : MonoBehaviour
{
    private Transform _planet;

    private Transform _transformToRotate;

    [Inject]
    private void Construct(PlanetServicesProvider planetServices)
    {
        _planet = planetServices.PlanetPosition;
    }

    private void Awake()
    {
        _transformToRotate = transform;
    }

    private void FixedUpdate()
    {
        if (_planet == null)
            return;

        Quaternion rotation = Quaternion.FromToRotation(-_transformToRotate.up, _planet.position - _transformToRotate.position);
        _transformToRotate.rotation = rotation * _transformToRotate.rotation;
    }

    public void SetPlanetPosition(Transform planetPosition)
    {
        if (planetPosition == null)
        {
            throw new ArgumentNullException(nameof (planetPosition));
        }

        _planet = planetPosition;
    }
}
