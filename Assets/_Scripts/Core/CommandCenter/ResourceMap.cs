using System.Collections.Generic;

public class ResourceMap
{
    private readonly List<Resource> _availableResources;

    public ResourceMap()
    {
        _availableResources = new List<Resource>();
    }

    public bool HasResources => _availableResources.Count > 0;

    public void Add(Resource resource)
    {
        if (resource.IsAvailable == false || _availableResources.Contains(resource))
            return;

        _availableResources.Add(resource);
        resource.Reserved += OnResourceReserved;
    }

    public Resource GetResource()
    {
        return _availableResources[0];
    }

    private void Remove(Resource resource)
    {
        _availableResources.Remove(resource);
    }

    private void OnResourceReserved(Resource resource)
    {
        resource.Reserved -= OnResourceReserved;

        Remove(resource);
    }
}