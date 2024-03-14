public interface IPoolObject<T> 
{
    void BindToPool(IPool<T> pool);
    void OnGetFromPool();
}