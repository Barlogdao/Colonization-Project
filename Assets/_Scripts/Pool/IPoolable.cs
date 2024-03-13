public interface IPoolable<T> 
{
    void BindToPool(IPool<T> pool);
    void OnGetFromPool();
}