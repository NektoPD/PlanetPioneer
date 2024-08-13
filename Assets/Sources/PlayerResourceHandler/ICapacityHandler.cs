using System;
using System.Collections;
using System.Collections.Generic;

public interface ICapacityHandler
{
    public bool IsMaxCapacityReached(Type resourceType);
    public event Action MaxCapacityUpdated;
    public IReadOnlyDictionary<Type, int> MaxCapacityConstaints { get; }
}
