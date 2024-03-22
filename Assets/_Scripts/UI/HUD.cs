using TMPro;
using UnityEngine;
using Zenject;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceAmount;
    [SerializeField] private Canvas _canvas;

    private ICommandCenterNotifier _commandCenter;
    private Selector _selector;

    [Inject]
    private void Construct(Selector selector)
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
            _commandCenter.ResourceAmountChanged += OnResourceAmountChanged;
        }
    }

    private void OnDeselected()
    {
        Hide();

        if (_commandCenter != null)
        {
            _commandCenter.ResourceAmountChanged -= OnResourceAmountChanged;
            _commandCenter = null;
        }
    }

    private void OnResourceAmountChanged(int value)
    {
        _resourceAmount.text = value.ToString();
    }

    private void Show() => _canvas.enabled = true;
    private void Hide() => _canvas.enabled = false;
}