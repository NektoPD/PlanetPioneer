using System;
using Agava.YandexGames;

public interface IResourceCatcher
{
    public event Action<Resource> CatchedResource;
    public event Action StartedGatheringResources;
    public event Action StoppedGatheringResources;
}