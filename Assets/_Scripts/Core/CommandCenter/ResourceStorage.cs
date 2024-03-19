using System;

public class ResourceStorage
{
    private int _amount;

    public ResourceStorage()
    {
        _amount = 0;
    }

    public bool CanSpend(int amount)
    {
        return _amount >= amount;
    }

    public void Add(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException($"value {nameof(amount)} cant be negative");

        _amount += amount;
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

        _amount -= amount;
    }
}
