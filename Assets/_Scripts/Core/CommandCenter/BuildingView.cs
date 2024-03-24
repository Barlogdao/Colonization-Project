using UnityEngine;
using DG.Tweening;
using EPOOutline;

public class BuildingView : View
{
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _spawnDuration;
    [SerializeField] private Ease _ease;
    [SerializeField] private ParticleSystem _landingSmoke;

    private MeshRenderer[] _meshRenderers;
    private Outlinable _outlinable;

    private new void Awake()
    {
        base.Awake();

        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _outlinable = GetComponent<Outlinable>();

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