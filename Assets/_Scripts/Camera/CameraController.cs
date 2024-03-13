using DG.Tweening;
using RB.Extensions.Vector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [SerializeField] LayerMask _layerMask;
    [SerializeField] private float _reachDuration;

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("������");

            var mousePosition = Mouse.current.position.value;

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _layerMask))
            {
                transform.DOMove(hit.point.WithY(transform.position.y), _reachDuration);
            }
        }
    }

}
