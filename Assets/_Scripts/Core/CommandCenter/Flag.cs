using DG.Tweening;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class Flag : MonoBehaviour
{
    [SerializeField] Ease _ease;
    [SerializeField] float _settingDuration;
    [SerializeField] float _settingOffsetY;

    private MeshRenderer _meshRenderer;

    public bool IsSet => _meshRenderer.enabled;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        Unset();
    }

    public void Set(Vector3 position, Quaternion rotation)
    {
        _meshRenderer.enabled = true;
        transform.SetPositionAndRotation(position, rotation);
        transform.DOMoveY(transform.position.y - _settingOffsetY, _settingDuration).From().SetEase(_ease);
    }

    public void Unset()
    {
        _meshRenderer.enabled = false;
    }
}
