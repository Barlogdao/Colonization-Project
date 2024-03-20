using System;

public class ResourceStorage
{
    public ResourceStorage()
    {
        Amount = 0;
    }

    public event Action<int> AmountChanged;

    public int Amount { get; private set; }

    public bool CanSpend(int amount)
    {
        return Amount >= amount;
    }

    public void Add(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException($"value {nameof(amount)} cant be negative");

        Amount += amount;
        AmountChanged?.Invoke(Amount);
    }

    public bool TrySpend(int amount)
    {
        if (CanSpend(amount) == false)
            return false;

        Spend(amount);
        return true;
    }

    private void Spend(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException($"value {nameof(amount)} cant be negative");

        Amount -= amount;
        AmountChanged?.Invoke(Amount);
    }
}
