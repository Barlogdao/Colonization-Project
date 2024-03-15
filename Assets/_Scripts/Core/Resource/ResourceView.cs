using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ResourceView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _glowParticles;
    [SerializeField] private ParticleSystem _reserveMarkParticles;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnAppear()
    {
        _meshRenderer.enabled = false;
        _glowParticles.Play();
        _reserveMarkParticles.Stop();
    }

    public void OnReveal()
    {
        _meshRenderer.enabled = true;
    }

    public void OnReserve()
    {
        _reserveMarkParticles.Play();
    }

    public void OnHarvest()
    {
        _glowParticles.Stop();
        _reserveMarkParticles.Stop();
    }
}
