using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private float _spawnInterval;
    
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private IEnumerator Start()
    {
        WaitForSeconds interval = new(_spawnInterval);

        while (true)
        {
            yield return interval;

            Instantiate(_resourcePrefab,GetPosition(), Quaternion.identity);
        }
    }

    private Vector3 GetPosition()
    {
        Vector3 minimalPosition = _boxCollider.bounds.min;
        Vector3 maximalPosition = _boxCollider.bounds.max;

        return GetRandomVector3 (minimalPosition, maximalPosition);
    }

    private Vector3 GetRandomVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x,max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    } 
}
