using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class SpawnArea : MonoBehaviour
{
    [SerializeField] private float _maxSpawnAmount;
    [SerializeField] private LayerMask _layerMask;

    private MeshCollider _meshCollider;

    private void Awake()
    {
        _meshCollider = GetComponent<MeshCollider>();
    }

    public void SpawnResource(Resource resource, Transform planetTransform)
    {
        Bounds bounds = _meshCollider.bounds;

        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.center.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        Vector3 rayDirection = (planetTransform.position - randomPoint).normalized;

        RaycastHit hit;

        if (Physics.Raycast(randomPoint, rayDirection, out hit, Mathf.Infinity, _layerMask))
        {
            Vector3 surfacePoint = hit.point;
            resource.transform.position = surfacePoint;
        }
        else
        {
            resource.transform.position = randomPoint;
        }
    }
}

