using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchedResourceHandler : MonoBehaviour
{
    private readonly List<Resource> _ironCatched = new List<Resource>();
    private readonly List<Resource> _crystalCatched = new List<Resource>();

    public IReadOnlyCollection<Resource> IronCatched => _ironCatched;

    private Weapon _weapon;

    public event Action<int> IronAmountChanged; 

    public void AddResource(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException();


        switch (resource)
        {
            case Iron iron:
                _ironCatched.Add(resource);
                IronAmountChanged?.Invoke(_ironCatched.Count);
                break;
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException();

        _weapon = weapon;
        _weapon.ResourceCatcher.CatchedResource += AddResource;
    }
}
