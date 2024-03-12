using UnityEngine;

public abstract class PooledObject : MonoBehaviour, IPoolable<PooledObject>
{
    private IPool<PooledObject> _pool;
    public void BindToPool(IPool<PooledObject> pool)
    {
        _pool = pool;
    }

    protected void ReturnToPool()
    {
        OnReturnToPool();
        _pool.Release(this);
    }

    protected abstract void OnReturnToPool();
}