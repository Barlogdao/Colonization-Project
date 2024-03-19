using System;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceAmount;
    [SerializeField] private Canvas _canvas;

    private ICommandCenterNotifier _commandCenter;
    private Selector _selector;

    public void Initialize(Selector selector)
    {
        _selector = selector;

        _selector.Selected += OnSelected;
        _selector.Deselected += OnDeselected;
    }

    private void OnDestroy()
    {
        _selector.Selected -= OnSelected;
        _selector.Deselected -= OnDeselected;
    }

    private void OnSelected(ISelectable selectable)
    {
        if (selectable is ICommandCenterNotifier commandCenter)
        {
            Show();
            _commandCenter = commandCenter;
            _commandCenter.ResourceAmountChanged += OnResourceChanged;
        }
    }

    private void OnDeselected()
    {
        Hide();

        if (_commandCenter != null)
        {
            _commandCenter.ResourceAmountChanged -= OnResourceChanged;
            _commandCenter = null;
        }
    }

    private void OnResourceChanged(int value)
    {
        _resourceAmount.text = value.ToString();
    }

    private void Show()
    {
        _canvas.enabled = true;
    }

    private void Hide()
    {
        _canvas.enabled = false;
    }
}