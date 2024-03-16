using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceAmount;

    private ICommandCenterNotifier _commandCenter;

    public void Initialize(ICommandCenterNotifier commandCenter)
    {
        _commandCenter = commandCenter;
        _commandCenter.ResourceAmountChanged += OnResourceChanged;
    }

    private void OnDestroy()
    {
        _commandCenter.ResourceAmountChanged -= OnResourceChanged;
    }

    private void OnResourceChanged(int value)
    {
        _resourceAmount.text = value.ToString();
    }
}