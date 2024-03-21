using DG.Tweening;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] Ease _ease;
    [SerializeField] float _settingDuration;
    [SerializeField] float _settingOffsetY;
    public void Set(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        transform.DOMoveY(transform.position.y - _settingOffsetY, _settingDuration).From().SetEase(_ease);
    }
}
