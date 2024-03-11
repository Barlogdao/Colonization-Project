using System.Collections.Generic;
using UnityEngine;

public class ResourceMap
{
    private readonly List<Resource> _avaliableResources;

    public ResourceMap()
    {
        _avaliableResources = new List<Resource>();
    }

    public bool HasResources => _avaliableResources.Count > 0;

    public void Add(Resource resource)
    {
        if (resource.Avaliable == false || _avaliableResources.Contains(resource))
            return;

        _avaliableResources.Add(resource);
        resource.Reserved += OnResourceReserved;
    }

    public Resource GetResource()
    {
        return _avaliableResources[Random.Range(0,_avaliableResources.Count)];
    }

    private void Remove(Resource resource)
    {
        _avaliableResources.Remove(resource);
    }

    private void OnResourceReserved(Resource resource)
    {
        resource.Reserved -= OnResourceReserved;
        Remove(resource);
    }
}
