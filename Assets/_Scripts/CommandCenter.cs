using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private float _scanInterval; 

    private int _resourceAmount = 0;

    private Queue<Unit> _units;
    private ResourceMap _resourceMap;

    private bool HasAvailableUnit => _units.Count > 0;
    private bool HasHarvestableResource => _resourceMap.HasResources;
    private bool CanHarvestResource => HasAvailableUnit && HasHarvestableResource;

    public void Initialize()
    {
        _units = new Queue<Unit>();
        _resourceMap = new ResourceMap();
    }

    private IEnumerator Start()
    {
        while (enabled)
        {
            Scan();
            yield return new WaitForSeconds(_scanInterval);
        }
    }

    private void Update()
    {
        TryHarvestResource();
    }

    public void BindUnit(Unit unit)
    {
        _units.Enqueue (unit);
    }

    public void Scan()
    {
        Resource[] foundResources = _resourceScanner.GetScannedResources();

        foreach (var resource in foundResources)
        {
            _resourceMap.Add(resource);
        }
    }

    public void AcceptResource(Resource resource)
    {
        _resourceAmount += resource.Amount;
        resource.Remove();
    }

    private void TryHarvestResource()
    {
        if (CanHarvestResource == false)
            return;

        Unit unit = _units.Dequeue();
        Resource targetResource = _resourceMap.GetResource();

        targetResource.Reserve();
        unit.HarvestResource(this, targetResource);
    }
}
