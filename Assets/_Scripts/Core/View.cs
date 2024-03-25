using DG.Tweening;
using UnityEngine;

public class View : MonoBehaviour
{
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _spawnDuration;
    [SerializeField] private Ease _ease;
    [SerializeField] private ParticleSystem _landingSmoke;

    public void ShowSpawn()
    {
        transform.DOLocalMoveY(_spawnHeight, _spawnDuration).From().SetEase(_ease).OnComplete(() => _landingSmoke.Play());
    }
}