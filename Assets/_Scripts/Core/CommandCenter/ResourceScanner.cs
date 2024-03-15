using System.Linq;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private ScannerVisual _visual; 

    private ResourceMap _resourceMap;

    public void Initialize(ResourceMap resourceMap)
    {
        _resourceMap = resourceMap;
    }

    public void Scan()
    {
        _visual.DoScan(_scanRadius);

        var resources = GetScannedResources();

        foreach (Resource resource in resources)
        {
            if (resource.IsRevealed == false)
            {
                resource.Reveal();
            }

            _resourceMap.Add(resource);
        }
    }

    private Resource[] GetScannedResources()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayer);

        Resource[] resources = colliders.Where(c => c.TryGetComponent(out Resource resource))
            .Select(c => c.GetComponent<Resource>())
            .ToArray();

        return resources;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}
