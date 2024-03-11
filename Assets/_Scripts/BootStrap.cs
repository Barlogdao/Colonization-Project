using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BootStrap : MonoBehaviour
{
    [SerializeField] private CommandCenter _base;

    [SerializeField] private List<Unit> _units;


    private void Awake()
    {
        _base.Initialize();

        foreach (var unit in _units)
        {
            _base.BindUnit(unit);
        }
    }

    private void OnValidate()
    {
        _units = FindObjectsByType<Unit>(sortMode: FindObjectsSortMode.None).ToList();
    }
}