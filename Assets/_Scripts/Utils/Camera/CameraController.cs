using RB.Extensions.Vector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.value;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _layerMask))
            {
                transform.position = hit.point.WithY(transform.position.y);
            }
        }
    }
}
