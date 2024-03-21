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

    public bool IsAvaliable { get; private set; } = false;
    private Action<Vector3, Quaternion> _buildCallback;

    [Inject]
    private void Construct(InputController inputController)
    {
        _input = inputController;
    }

    public void EnterBuildMode()
    {
        _input.CancelPressed += ExitBuildMode;
    }

    private void ExitBuildMode()
    {
        _input.CancelPressed -= ExitBuildMode;

        _blueprint.Deactivate();
        _buildCallback = null;
        IsAvaliable = false;
    }


    public void StartBuild(BuildingView view, Action<Vector3, Quaternion> buildCallback)
    {
        _blueprint.Activate(view);
        _buildCallback = buildCallback;
        IsAvaliable = true;
        EnterBuildMode();
    }

    private void Update()
    {
        if (_blueprint.IsActive == false)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, _placementLayer) == true)
        {

            _blueprint.Move(hitInfo.point);

            RotationHandle();
            _blueprint.PlacementCheck();

            HandleInput();
        }
    }

    private void HandleInput()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            LeftClickHandle();
        }
    }

    private void RotationHandle()
    {
        float mouseWheelInput = Mouse.current.scroll.y.value;
        _blueprint.Rotate(mouseWheelInput * _rotateSpeed);
    }

    private void LeftClickHandle()
    {
        if (_blueprint.CanPlace == true)
        {
            _buildCallback.Invoke(_blueprint.transform.position, _blueprint.transform.rotation);
            ExitBuildMode();
        }
    }

    private void RightClickHandle()
    {
        ExitBuildMode();
    }
}
