using System.Linq;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private float _scanCooldown;

    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private ScannerVisual _visual;

    private ResourceMap _resourceMap;
    private float _elapsedTime;

    public void Initialize(ResourceMap resourceMap)
    {
        _resourceMap = resourceMap;
        _elapsedTime = 0f;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
    }

    public void Scan()
    {
        if (_elapsedTime < _scanCooldown)
            return;

        _visual.DoScan(_scanRadius);

        foreach (Resource resource in GetScannedResources())
        {
            if (resource.IsRevealed == false)
                resource.Reveal();

            _resourceMap.Add(resource);
        }
        
        _elapsedTime = 0f;
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
