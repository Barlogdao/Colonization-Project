using UnityEngine;
using UnityEngine.InputSystem;
using RB.Extensions.Component;
using System;

public class Selector : MonoBehaviour
{
    private Camera _camera;
    private ISelectable _selectable;

    public event Action<ISelectable> Selected;
    public event Action Deselected;

    private Ray ScreenPointRay { get => _camera.ScreenPointToRay(Mouse.current.position.value); }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySelect();
        }
    }

    private void TrySelect()
    {
        if (Physics.Raycast(ScreenPointRay, out RaycastHit hit) == false)
            return;

        if (hit.collider.TryGetComponentInParent(out ISelectable selectable) == false)
        {
            _selectable?.Deselect();
            Deselected?.Invoke();
            _selectable = null;
            return;
        }

        if (selectable == _selectable)
        {
            return;
        }

        _selectable?.Deselect();
        Deselected?.Invoke();
        _selectable = selectable;
        Selected?.Invoke(_selectable);
        _selectable.Select();
    }
}