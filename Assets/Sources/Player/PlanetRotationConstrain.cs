using System;
using UnityEngine;

public class PlanetRotationConstrain : MonoBehaviour
{
    [SerializeField] private Transform _planet;

    private Transform _transformToRotate;

    private void Awake()
    {
        _transformToRotate = transform;
    }

    private void FixedUpdate()
    {
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
