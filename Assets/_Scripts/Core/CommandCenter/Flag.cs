using DG.Tweening;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] Ease _ease;
    [SerializeField] float _settingDuration;
    [SerializeField] float _settingOffsetY;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Transform _view;

    public bool IsSet => _meshRenderer.enabled;

    private void Awake()
    {
        _view = _meshRenderer.transform;

        Unset();
    }

    public void Set(Vector3 position, Quaternion rotation)
    {
        _meshRenderer.enabled = true;
        transform.SetPositionAndRotation(position, rotation);
        _view.DOLocalMoveY(transform.position.y - _settingOffsetY, _settingDuration).From().SetEase(_ease);
    }

    public void Unset()
    {
        _meshRenderer.enabled = false;
    }
}
