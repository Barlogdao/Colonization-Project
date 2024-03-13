using System.Linq;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _resourceLayer;

    public Resource[] GetScannedResources()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayer);

        Resource[] resources = colliders.Where(c => c.TryGetComponent(out Resource resource))
            .Select(c => c.GetComponent<Resource>())
            .ToArray();

        foreach (Resource resource in resources)
        {
            if (resource.IsRevealed == false)
            {
                resource.Reveal();
            }
        }

        return resources;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}
