using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage
{
    private int _amount;

    public ResourceStorage()
    {
        _amount = 0;
    }

    public bool HasEnough(int amount)
    {
        return _amount >= amount;
    }

    public void Add(int amount)
    {
        _amount += amount;
    }

    public void Spend(int amount)
    {
        _amount -= amount;
    }
}
