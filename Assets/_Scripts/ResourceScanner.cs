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

        return resources;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}
