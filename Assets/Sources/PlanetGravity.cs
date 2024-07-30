using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class PlanetGravity : MonoBehaviour
{
    [SerializeField] private float _gravityConstant = 12f;
    
    private Transform _planet;
    private Rigidbody _rigidbody;

    [Inject]
    private void Consturct(PlanetServicesProvider planetServices)
    {
        _planet = planetServices.PlanetPosition;

    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_planet == null)
            return;

        Vector3 toCenter = _planet.position - transform.position;
        toCenter.Normalize();

        _rigidbody.AddForce(toCenter * _gravityConstant, ForceMode.Acceleration);
    }
}