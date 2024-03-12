using UnityEngine;

public class RotateModule : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationStep;
    [SerializeField] private float _rotateSpeed;

    private void Update()
    {
        transform.Rotate(_rotateSpeed * Time.deltaTime * _rotationStep);
    }
}