using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private float _spawnInterval;
    [SerializeField, Min(1f)] private float _spawnZoneRadius;
    [SerializeField, Min(1)] private int _maxResourceAmount;

    private Pool<PoolObject> _resourcePool;

    private void Awake()
    {
        _resourcePool = new Pool<PoolObject>(_container, _resourcePrefab);
    }

    private IEnumerator Start()
    {
        WaitForSeconds spawnInterval = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return spawnInterval;

            Spawn();
        }
    }

    private void Spawn()
    {
        if (_resourcePool.TakenObjectsCount >= _maxResourceAmount)
            return;

        PoolObject resource = _resourcePool.Get();
        resource.transform.position = GetPosition();
    }

    private Vector3 GetPosition()
    {
        Vector3 position = transform.position + (Random.insideUnitSphere * _spawnZoneRadius);
        position.y = transform.position.y;

        return position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _spawnZoneRadius);
    }
}
