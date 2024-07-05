using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceCatcher : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _distance = 5;
    [SerializeField] private LayerMask _resourceMask;
    [SerializeField] private Transform _gunPosition;
    [SerializeField] private float _lerpDuration;
    [SerializeField] private Vector3 _targetScale;

    private Weapon _weapon;

    public event Action<Resource> CatchedResource;

    public IEnumerator LerpToGunPosition(Transform target)
    {
        if (_gunPosition == null)
            yield break;

        Vector3 startPosition = target.position;
        Vector3 startScale = target.localScale;
        float timeElapsed = 0f;

        while (timeElapsed < _lerpDuration)
        {
            float lerpFactor = timeElapsed / _lerpDuration;

            target.position = Vector3.Lerp(startPosition, _gunPosition.position, lerpFactor);
            target.localScale = Vector3.Lerp(startScale, _targetScale, lerpFactor);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        target.position = _gunPosition.position;
        target.localScale = _targetScale;

        target.gameObject.SetActive(false);
    }

    public void SetWeapon(Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException();

        _weapon = weapon;
        _weapon.ShootButtonPressed += Shoot;
    }

    private void Shoot()
    {
        if (_weapon == null)
            return;

        Vector3 shootOrigin = transform.position + transform.forward * _distance;
        RaycastHit[] hits = Physics.SphereCastAll(shootOrigin, _radius, transform.forward, _distance);

        var resources = hits.Select(hit => hit.collider.GetComponent<Resource>()).Where(resource => resource != null);

        foreach (Resource resource in resources)
        { 
            StartCoroutine(LerpToGunPosition(resource.transform));
            CatchedResource?.Invoke(resource);
        }
    }

    private void OnDisable()
    {
        _weapon.ShootButtonPressed -= Shoot;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(_gunPosition.position + _gunPosition.forward * _distance, _radius);
    }
}
