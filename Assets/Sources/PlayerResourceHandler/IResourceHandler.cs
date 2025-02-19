using System;
using System.Collections.Generic;

public interface IResourceHandler
{
    public event Action ResourceAmountChanged;
    public event Action ResourcesCleared;

    public int CurrentIronAmount { get; }
    public int CurrentCrystalAmount { get; }
    public int CurrentPlantAmount { get; }
    public int CurrentAlienArtifactAmount { get; }
    public IReadOnlyDictionary<Type, int> CurrentResourceCatched { get; }
    public void SetResourceAmount(Type resourceType, int amount);
}