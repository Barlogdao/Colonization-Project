using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Reserved;

    [field: SerializeField, Min(1)] public int Amount { get; private set; }

    public bool Avaliable { get; private set; } = true;

    public void Reserve()
    {
        Avaliable = false;
        Reserved?.Invoke(this);
    }

    public void Harvest(Transform harvester, Vector3 placePosition)
    {
        transform.SetParent(harvester);
        transform.position = placePosition;
    }

    public void Unload()
    {
        Destroy(gameObject);
    }

}
