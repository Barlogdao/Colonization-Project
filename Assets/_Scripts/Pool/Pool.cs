using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : IPool<T> where T : MonoBehaviour, IPoolObject<T>
{
    private const bool Taken = false;
    private const bool Available = true;

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

    private bool HasAvailableObject => _pool.Any(o => o.Value == Available);

    public T Get()
    {
        T poolObject = GetFromPull();

        poolObject.gameObject.SetActive(true);
        poolObject.OnGetFromPool();

        return poolObject;
    }

    public void Return(T poolObject)
    {
        poolObject.transform.parent = _container;
        poolObject.gameObject.SetActive(false);

        ReturnToPull(poolObject);
    }

    private T GetFromPull()
    {
        if (HasAvailableObject == false)
        {
            CreateObject();
        }

        T poolObject = _pool.First(o => o.Value == Available).Key;
        MarkAsTaken(poolObject);

        return poolObject;
    }

    private void ReturnToPull(T poolObject)
    {
        MarkAsAvailable(poolObject);
    }

    private void CreateObject()
    {
        T objectInstance = Object.Instantiate(_prefab, _container);

        objectInstance.BindToPool(this);
        _pool.Add(objectInstance, Available);
    }

    private void MarkAsAvailable(T poolObject) => _pool[poolObject] = Available;
    private void MarkAsTaken(T poolObject) => _pool[poolObject] = Taken;
}
