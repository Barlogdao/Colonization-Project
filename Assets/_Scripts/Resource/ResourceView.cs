using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ResourceView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _glowParticles;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnAppear()
    {
        _meshRenderer.enabled = false;
        _glowParticles.Play();
    }

    public void OnReveal()
    {
        _meshRenderer.enabled = true;
    }

    public void OnReserve()
    {

    }

    public void OnHarvest()
    {
        _glowParticles.Stop();
    }
}
