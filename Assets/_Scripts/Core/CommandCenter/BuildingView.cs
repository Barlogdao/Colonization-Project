using UnityEngine;
using DG.Tweening;
using EPOOutline;

[RequireComponent (typeof(SphereCollider))]
public class BuildingView : MonoBehaviour
{
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _spawnDuration;
    [SerializeField] private Ease _ease;

    [SerializeField] private ParticleSystem _landingSmoke;

    private MeshRenderer[] _meshRenderers;
    private Outlinable _outlinable;
    public SphereCollider SphereCollider { get; private set; }

    private void Awake()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        SphereCollider = GetComponent<SphereCollider>();
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


    public void ChangeMaterials(Material material)
    {
        foreach (var renderer in _meshRenderers)
        {
            renderer.material = material;
        }
    }

    public void ShowSpawn()
    {
        transform.DOMoveY(_spawnHeight, _spawnDuration).From().SetEase(_ease).OnComplete(() => _landingSmoke.Play());
    }
}
