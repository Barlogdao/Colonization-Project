using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceAmount;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _scannerIcon;

    [SerializeField] private Color _activeScanner;
    [SerializeField] private Color _unactiveScanner;

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

    private void Update()
    {
        if (_commandCenter != null) 
        {
            _scannerIcon.fillAmount =_commandCenter.ScannerCooldownProgress;

            _scannerIcon.color = _scannerIcon.fillAmount == 1f ? _activeScanner : _unactiveScanner;
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