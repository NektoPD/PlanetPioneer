using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private Transform _transform;
    private bool _isAbsorbed = false;

    public Vector3 InitScale { get; private set; }
    public bool IsAbsorbed => _isAbsorbed;

    private void Awake()
    {
        _transform = transform;
        InitScale = transform.localScale;
    }

    public void SetInitScale()
    {
        _transform.localScale = InitScale;
    }

    public void SetAbsorbedToFalse()
    {
        if(_isAbsorbed == true)
            _isAbsorbed = false;
    }

    public void SetAbsorbedToTrue()
    {
        if (_isAbsorbed == false)
            _isAbsorbed = true;
    }
}
