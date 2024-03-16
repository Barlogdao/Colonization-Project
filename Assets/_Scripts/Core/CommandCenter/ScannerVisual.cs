using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ScannerVisual : MonoBehaviour
{
    [SerializeField] private float _scanDuration = 0.5f;
    [SerializeField] private Ease _ease;

    private MeshRenderer _renderer;
    private Vector3 _originScale;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originScale = transform.localScale;

        Hide();
    }

    public void DoScan(float scanRadius)
    {
        Show();

        float viewScale = scanRadius * 2;
        transform.DOScale(viewScale, _scanDuration).SetEase(_ease).OnComplete(Hide);
    }

    private void Show() => _renderer.enabled = true;

    private void Hide()
    {
        _renderer.enabled = false;
        transform.localScale = _originScale;
    }
}
