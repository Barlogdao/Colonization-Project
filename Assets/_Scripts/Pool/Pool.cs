using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
{
    private const bool Taken = false;
    private const bool Avaliable = true;

    private readonly Dictionary<T, bool> _pool;
    private readonly Transform _container;
    private readonly T _prefab;

    public Pool(Transform container, T prefab)
    {
        _pool = new Dictionary<T, bool>();

        _container = container;
        _prefab = prefab;
    }

    public int TakenObjectsCount => _pool.Count(o => o.Value == Taken);

    private bool HasAvaliableObject => _pool.Any(o => o.Value == Avaliable);

    public T Get()
    {
        T pooledObject = GetFromPull();

        pooledObject.gameObject.SetActive(true);
        pooledObject.OnGetFromPool();

        return pooledObject;
    }

    public void Release(T poolObject)
    {
        poolObject.transform.parent = _container;
        poolObject.gameObject.SetActive(false);

        ReturnToPull(poolObject);
    }

    private T GetFromPull()
    {
        if (HasAvaliableObject == false)
        {
            CreateObject();
        }

        T obj = _pool.First(o => o.Value == Avaliable).Key;
        MarkAsTaken(obj);

        return obj;
    }

    private void ReturnToPull(T obj)
    {
        MarkAsAvaliable(obj);
    }

    private void CreateObject()
    {
        T objectInstance = GameObject.Instantiate(_prefab, _container);

        objectInstance.BindToPool(this);

        _pool.Add(objectInstance, Avaliable);
    }

    private void MarkAsAvaliable(T obj) => _pool[obj] = Avaliable;
    private void MarkAsTaken(T obj) => _pool[obj] = Taken;
}
