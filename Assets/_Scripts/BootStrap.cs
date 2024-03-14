using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BootStrap : MonoBehaviour
{
    [SerializeField] private CommandCenter _commandCenter;

    [SerializeField] private List<Unit> _units;


    private void Awake()
    {
        _commandCenter.Initialize();

        foreach (var unit in _units)
        {
            _commandCenter.BindUnit(unit);
        }
    }

    private void OnValidate()
    {
        _units = FindObjectsByType<Unit>(sortMode: FindObjectsSortMode.None).ToList();
    }
}