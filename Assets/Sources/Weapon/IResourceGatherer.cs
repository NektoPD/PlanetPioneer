using System;

public interface IResourceGatherer
{
    public event Action StartedGatheringResources;
    public event Action StopedGatheringResources;
}