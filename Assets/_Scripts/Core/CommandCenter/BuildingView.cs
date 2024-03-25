using UnityEngine;
using DG.Tweening;
using EPOOutline;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BuildingView : View
{
    private MeshRenderer[] _meshRenderers;
    private Outlinable _outlinable;
    private Collider _collider;
    private int _collisionAmount = 0;

    public bool IsCollide => _collisionAmount > 0;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _outlinable = GetComponent<Outlinable>();

        _collider.isTrigger = true;

        HideOutline();
    }

    private void OnTriggerEnter(Collider other)
    {
        _collisionAmount++;
    }

    private void OnTriggerExit(Collider other)
    {
        _collisionAmount--;
    }

    public void ShowOutline()
    {
        _outlinable.enabled = true;
    }

    public void HideOutline()
    {
        _outlinable.enabled = false;
    }


    public void SetMaterial(Material material)
    {
        foreach (MeshRenderer renderer in _meshRenderers)
            renderer.material = material;
    }
}