using System;

public interface ICapacityHandler
{
    public bool IsMaxCapacityReached(Type resourceType);
    
}
