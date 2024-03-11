using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private float _scanInterval; 

    private int _resourceAmount = 0;

    private Queue<Unit> _units;
    private List<Resource> _avaliableResources;

    private bool HasFreeUnit => _units.Count > 0;
    private bool HasFreeResource => _avaliableResources.Count > 0;
    private bool CanHarvestResource => HasFreeUnit && HasFreeResource;

    public void Initialize()
    {
        _units = new Queue<Unit>();
        _avaliableResources = new List<Resource>();
    }

    private IEnumerator Start()
    {
        while (enabled)
        {
            TryHarvestResource();
            yield return new WaitForSeconds(_scanInterval);
            Scan();
        }
    }

    public void BindUnit(Unit unit)
    {
        _units.Enqueue (unit);
        unit.BindToCommandCenter(this);
    }

    public void Scan()
    {
        var avaliableResources = _resourceScanner.GetScannedResources();

        foreach (var resource in avaliableResources)
        {
            AddFreeResource(resource);
        }
    }

    public void CollectResource(int amount)
    {
        _resourceAmount += amount;
    }

    public void ReturnUnit(Unit unit)
    {
        _units.Enqueue(unit);
    }

    private void TryHarvestResource()
    {
        if (CanHarvestResource == false)
            return;

        Unit unit = _units.Dequeue();
        Resource targetResource = _avaliableResources[0];
        targetResource.Reserve();

        unit.HarvestResource(targetResource);
    }

    private void AddFreeResource(Resource resource)
    {
        _avaliableResources.Add(resource);
        resource.Reserved += OnResourceReserved;
    }

    private void OnResourceReserved(Resource resource)
    {
        resource.Reserved -= OnResourceReserved;
        _avaliableResources.Remove(resource);
    }
}
