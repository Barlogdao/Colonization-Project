using UnityEngine;
using RB.Extensions.Component;
using System;
using Zenject;

public class Selector : ITickable
{
    private readonly InputController _input;
    private ISelectable _selectable;

    public event Action<ISelectable> Selected;
    public event Action Deselected;

    public Selector(InputController input)
    {
        _input = input;
        _selectable = null;
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

        if (hit.collider.TryGetComponentInParent(out ISelectable selectable) == true)
        {
            if (selectable == _selectable)
                return;
            
            Select(selectable);
        }
        else
        {
            DeselectCurrent(); ;
        }
    }

    private void Select(ISelectable selectable)
    {
        DeselectCurrent();

        _selectable = selectable;
        Selected?.Invoke(_selectable);
        _selectable.Select();
    }

    private void DeselectCurrent()
    {
        Deselected?.Invoke();
        _selectable?.Deselect();
        _selectable = null;
    }
}