namespace RB.Extensions.Component
{
    public static class ComponentExtension
    {
        public static bool TryGetComponentInParent<T>(this UnityEngine.Component @this, out T component)
        {
            component = @this.GetComponentInParent<T>();

            return component != null;
        }
    }
}