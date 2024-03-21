using RB.Extensions.Vector;
using System;
using UnityEngine;
using Zenject;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _moveSpeed;
    [SerializeField, Range(0f, 1f)] private float _edgeTolerance;
    private InputController _input;
    private Camera _camera;
    private Selector _selector;

    [Inject]
    private void Construct(InputController input, Selector selector)
    {
        _input = input;
        _selector = selector;
    }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _selector.Selected += OnSelected;
    }

    private void OnDisable()
    {
        _selector.Selected -= OnSelected;
    }

    private void Update()
    {
        HandleMouseClick();
        HandleEdgeMove();
    }


    private void OnSelected(ISelectable selectable)
    {
        transform.position = selectable.Position;
    }

    private void HandleMouseClick()
    {
        if (_input.IsRightMouseButtonClicked == true)
        {
            if (Physics.Raycast(_input.ScreenPointRay, out RaycastHit hit, 1000f, _layerMask))
            {
                transform.position = hit.point.WithY(transform.position.y);
            }
        }
    }

    private void HandleEdgeMove()
    {
        Vector3 mouseViewportPosition = _camera.ScreenToViewportPoint(_input.MousePosition);
        Vector3 direction = GetDirection(mouseViewportPosition);

        if (direction != Vector3.zero) 
        {
            transform.Translate(direction * _moveSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetDirection(Vector3 mouseViewportPosition) 
    {
        Vector3 direction = Vector3.zero;

        if (mouseViewportPosition.x > 1 - _edgeTolerance)
            direction += Vector3.right;
        else if(mouseViewportPosition.x < _edgeTolerance)
            direction += Vector3.left;

        if (mouseViewportPosition.y > 1 - _edgeTolerance)
            direction += Vector3.forward;
        else if( mouseViewportPosition.y < _edgeTolerance)
            direction += Vector3.back;

        return direction;
    }
}
