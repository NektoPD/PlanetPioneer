using System;
using UnityEngine;
using Zenject;

public class ResourceIndicator : MonoBehaviour
{
    [SerializeField] private IronResourceSpawner _ironResourceSpawner;
    [SerializeField] private CrystalResourceSpawner _crystalResourceSpawner;
    [SerializeField] private PlantResourceSpawner _plantResourceSpawner;
    [SerializeField] private AlienArtifactResourceSpawner _alienArtifactResourceSpawner;

    private IResourceCatcher _resourceCatcher;

    [Inject]
    private void Construct(IResourceCatcher resourceCatcher)
    {
        _resourceCatcher = resourceCatcher;
        _resourceCatcher.CatchedResource += IndicateResourceType;
    }

    private void OnDisable()
    {
        _resourceCatcher.CatchedResource -= IndicateResourceType;
    }

    private void IndicateResourceType(Resource resource)
    {
        var resourceType = resource.GetType();
        
        if (resourceType == typeof(Iron))
            _ironResourceSpawner.ReturnResourceToPull(resource);
        else if (resourceType == typeof(Crystal))
            _crystalResourceSpawner.ReturnResourceToPull(resource);
        else if (resourceType == typeof(Plant))
            _plantResourceSpawner.ReturnResourceToPull(resource);
        else if (resourceType == typeof(AlienArtifact))
            _alienArtifactResourceSpawner.ReturnResourceToPull(resource);
    } 
}