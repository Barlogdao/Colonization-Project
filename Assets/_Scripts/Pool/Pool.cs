using System.Collections.Generic;
using UnityEngine;

public class Pool<T>: IPool<T> where T : MonoBehaviour, IPoolable<T>
{
    private readonly Queue<T> _pool;
    private readonly Transform _container;
    private readonly T _prefab;

    public Pool(Transform container, T prefab)
    {
        _pool = new Queue<T>();

        _container = container;
        _prefab = prefab;
    }

    public T Get()
    {
        if (_pool.Count == 0)
        {
            CreateObject();
        }

        T pooledObject = _pool.Dequeue();
        pooledObject.gameObject.SetActive(true);

        return pooledObject;
    }

    public void Release(T poolObject)
    {
        _pool.Enqueue(poolObject);
        poolObject.transform.parent = _container;

        poolObject.gameObject.SetActive(false);
    }

    private void CreateObject()
    {
        T pooledObject = GameObject.Instantiate(_prefab, _container);

        pooledObject.BindToPool(this);
        _pool.Enqueue(pooledObject);
    }
}
