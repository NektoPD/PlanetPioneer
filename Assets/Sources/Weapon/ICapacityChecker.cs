using System;

public interface ICapacityChecker
{
    public bool IsMaxCapacityReached(Type resourceType);
}
