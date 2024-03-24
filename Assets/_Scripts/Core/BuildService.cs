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

    private Action<Vector3, Quaternion> _onBlueprintSet;

    public bool IsAvaliable { get; private set; } = true;

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

    public void ActivateBuildMode(BuildingView view, Action<Vector3, Quaternion> onBlueprintSet)
    {
        _blueprint.Activate(view);
        _onBlueprintSet = onBlueprintSet;

        _input.CancelPressed += DeactivateBuildMode;
        IsAvaliable = false;
    }

    private void DeactivateBuildMode()
    {
        _blueprint.Deactivate();
        _onBlueprintSet = null;

        _input.CancelPressed -= DeactivateBuildMode;
        IsAvaliable = true;
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
            _onBlueprintSet.Invoke(_blueprint.transform.position, _blueprint.transform.rotation);
            DeactivateBuildMode();
        }
    }
}
