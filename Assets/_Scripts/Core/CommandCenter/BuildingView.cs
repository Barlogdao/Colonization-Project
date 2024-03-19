using DG.Tweening;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public class BuildingView : MonoBehaviour
{
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _ease;
    [SerializeField] private ParticleSystem _landingSmoke;

    private MeshRenderer[] _meshRenderers;
    public SphereCollider SphereCollider { get; private set; }

    private void Awake()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        SphereCollider = GetComponent<SphereCollider>();
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
        transform.DOMoveY(_spawnHeight, _duration).From().SetEase(_ease).OnComplete(() => _landingSmoke.Play());
    }
}
