using UnityEngine;
using RB.Extensions.Component;
using System;
using Zenject;

public class Selector: ITickable
{
    private InputController _input;
    private ISelectable _selectable;

    public event Action<ISelectable> Selected;
    public event Action Deselected;

    [Inject]
    private void Construct(InputController input)
    {
        _input = input;
    }

    public void Tick()
    {
        if (_input.IsLeftMouseButtonClicked == true)
        {
            TrySelect();
        }
    }

    private void TrySelect()
    {
        if (Physics.Raycast(_input.ScreenPointRay, out RaycastHit hit) == false)
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