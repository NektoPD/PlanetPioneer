using System;
using System.Collections.Generic;

public interface ICapacityHandler
{
    public IReadOnlyDictionary<Type, int> MaxCapacityConstaints { get; }
    public bool IsMaxCapacityReached(Type resourceType);
}