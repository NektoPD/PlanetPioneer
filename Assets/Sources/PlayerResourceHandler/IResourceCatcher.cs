using System;

public interface IResourceCatcher
{
    public event Action<Resource> CatchedResource;
    public event Action StartedGatheringResources;
    public event Action StoppedGatheringResources;
}