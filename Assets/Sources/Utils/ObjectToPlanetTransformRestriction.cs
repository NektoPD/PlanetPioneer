using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToPlanetTransformRestriction : MonoBehaviour
{
    [SerializeField] private Transform _planetPosition;

    private Transform _position;

    private void Awake()
    {
        _position = transform;
    }

    private void Update()
    {
        
    }
}
