using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputController : ITickable, IDisposable
{
    private readonly PlayerInput _playerInput;

    public event Action ScanPressed;
    public event Action BuildPressed;

    public Vector2 CameraMovementInput { get; private set; }
    public bool IsLeftMouseButtonClicked => Mouse.current.leftButton.wasPressedThisFrame;
    public bool IsRightMouseButtonClicked => Mouse.current.rightButton.wasPressedThisFrame;
    public Vector2 MousePosition => Mouse.current.position.value;
    public Ray ScreenPointRay => Camera.main.ScreenPointToRay(MousePosition);

    public InputController()
    {
        _playerInput = new PlayerInput();

        _playerInput.Enable();
        _playerInput.Game.Scan.performed += OnScanPerformed;
        _playerInput.Game.Build.performed += OnBuildPerformed;
    }

    private void OnBuildPerformed(InputAction.CallbackContext context)
    {
        BuildPressed?.Invoke();
    }

    private void OnScanPerformed(InputAction.CallbackContext context)
    {
        ScanPressed?.Invoke();
    }

    public void Tick()
    {
        CameraMovementInput = _playerInput.Game.CameraMovement.ReadValue<Vector2>();
    }

    public void Dispose()
    {
        _playerInput.Game.Scan.performed -= OnScanPerformed;
        _playerInput.Game.Build.performed -= OnBuildPerformed;
        _playerInput.Disable();
    }
}
