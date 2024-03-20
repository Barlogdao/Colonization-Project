using RB.Extensions.Vector;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private InputController _input;

    [Inject]
    private void Construct(InputController input)
    {
        _input = input;
    }

    private void Update()
    {
        if (_input.IsRightMouseButtonClicked == true)
        {
            if (Physics.Raycast(_input.ScreenPointRay, out RaycastHit hit, 1000f, _layerMask))
            {
                transform.position = hit.point.WithY(transform.position.y);
            }
        }
    }
}
