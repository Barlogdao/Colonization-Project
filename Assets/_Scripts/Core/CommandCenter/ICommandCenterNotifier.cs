using System;

public interface ICommandCenterNotifier
{
    public event Action<int> ResourceAmountChanged;

    public float ScannerCooldownProgress { get; }
}