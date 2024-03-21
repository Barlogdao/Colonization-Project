using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class BuildService : MonoBehaviour
{
    [SerializeField] private Blueprint _blueprint;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private LayerMask _placementLayer;

    private InputController _input;
    private Camera _camera;

    public bool IsAvaliable { get; private set; } = false;
    private Action<Vector3, Quaternion> _buildCallback;

    [Inject]
    private void Construct(InputController inputController)
    {
        _input = inputController;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_blueprint.IsActive == false)
            return;

        if (Physics.Raycast(_input.ScreenPointRay, out RaycastHit hit, _camera.farClipPlane, _placementLayer) == true)
        {
            _blueprint.Move(hit.point);

            RotationHandle();

            HandleInput();
        }
    }

    public void EnterBuildMode(BuildingView view, Action<Vector3, Quaternion> buildCallback)
    {
        _blueprint.Activate(view);
        _buildCallback = buildCallback;

        _input.CancelPressed += ExitBuildMode;
        IsAvaliable = true;
    }

    private void ExitBuildMode()
    {
        _blueprint.Deactivate();
        _buildCallback = null;

        _input.CancelPressed -= ExitBuildMode;
        IsAvaliable = false;
    }

    private void RotationHandle()
    {
        float mouseWheelInput = Mouse.current.scroll.y.value;
        float angle = mouseWheelInput * _rotateSpeed;

        _blueprint.Rotate(angle);
    }

    private void HandleInput()
    {
        if (_input.IsLeftMouseButtonClicked && _blueprint.CanPlace)
        {
            _buildCallback.Invoke(_blueprint.transform.position, _blueprint.transform.rotation);
            ExitBuildMode();
        }
    }
}
