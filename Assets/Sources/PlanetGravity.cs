using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetGravity : MonoBehaviour
{
    [SerializeField] private float _gravityConstant = 12f;
    [SerializeField] private Transform _planet;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
    }

    private void FixedUpdate()
    {
        Vector3 toCenter = _planet.position - transform.position;
        toCenter.Normalize();

        _rigidbody.AddForce(toCenter * _gravityConstant, ForceMode.Acceleration);
    }
}