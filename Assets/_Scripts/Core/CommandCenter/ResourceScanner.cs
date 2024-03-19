using System.Linq;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private float _scanCooldown;
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private ScannerVisual _visual;

    private CooldownTimer _scannerTimer;
    private ResourceMap _resourceMap;

    public void Initialize(ResourceMap resourceMap)
    {
        _resourceMap = resourceMap;
        _scannerTimer = new CooldownTimer(_scanCooldown);
    }

    private void Update()
    {
        _scannerTimer.Update();
    }

    public void Scan()
    {
        if (_scannerTimer.IsReady == false)
            return;

        _visual.DoScan(_scanRadius);

        foreach (Resource resource in GetScannedResources())
        {
            if (resource.IsRevealed == false)
                resource.Reveal();

            _resourceMap.Add(resource);
        }

        _scannerTimer.Reset();
    }

    private Resource[] GetScannedResources()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayer);

        Resource[] resources = colliders.Where(c => c.TryGetComponent(out Resource resource))
            .Select(c => c.GetComponent<Resource>())
            .ToArray();

        return resources;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}
