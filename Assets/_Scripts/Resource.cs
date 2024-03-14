using System;
using UnityEngine;

public class Resource : PooledObject
{
    [SerializeField] ResourceView _view;

    public event Action<Resource> Reserved;

    [field: SerializeField, Min(1)] public int Amount { get; private set; }

    public bool IsAvailable { get; private set; } = true;
    public bool IsRevealed { get; private set; } = false;

    public void Reveal()
    {
        _view.OnReveal();
        IsRevealed = true;
    }

    public void Reserve()
    {
        _view.OnReserve();
        IsAvailable = false;
        Reserved?.Invoke(this);
    }

    public void Harvest(Transform harvester, Vector3 placePosition)
    {
        _view.OnHarvest();
        transform.SetParent(harvester);
        transform.position = placePosition;
    }

    public void Remove()
    {
        ReturnToPool();
    }

    public override void OnGetFromPool()
    {
        _view.OnAppear();
        IsAvailable = true;
        IsRevealed = false;
    }

    protected override void OnReturnToPool()
    {
        IsAvailable = false;
    }
}