using UnityEngine;

public abstract class PooledObject : MonoBehaviour, IPoolObject<PooledObject>
{
    private IPool<PooledObject> _pool;

    public void BindToPool(IPool<PooledObject> pool)
    {
        _pool = pool;
    }

    protected void ReturnToPool()
    {
        OnReturnToPool();
        _pool.Return(this);
    }

    public abstract void OnGetFromPool();
    protected abstract void OnReturnToPool();
}