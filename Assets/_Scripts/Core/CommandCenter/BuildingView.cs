using UnityEngine;
using DG.Tweening;
using EPOOutline;

[RequireComponent(typeof(Collider))]
public class BuildingView : MonoBehaviour
{
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _spawnDuration;
    [SerializeField] private Ease _ease;
    [SerializeField] private ParticleSystem _landingSmoke;

    private MeshRenderer[] _meshRenderers;
    private Outlinable _outlinable;
    private Collider _collider;

    public Bounds ColliderBounds =>_collider.bounds;

    private void Awake()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _outlinable = GetComponent<Outlinable>();
        _collider = GetComponent<Collider>();

        HideOutline();
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

    public void ShowSpawn()
    {
        transform.DOMoveY(_spawnHeight, _spawnDuration).From().SetEase(_ease).OnComplete(() => _landingSmoke.Play());
    }
}