using UnityEngine;

public abstract class PoolObject : MonoBehaviour, IPoolObject<PoolObject>
{
    private IPool<PoolObject> _pool;

    public void BindToPool(IPool<PoolObject> pool)
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