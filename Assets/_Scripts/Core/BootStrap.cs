using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BootStrap : MonoBehaviour
{
    [SerializeField] private CommandCenter _commandCenter;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private HUD _hud;

    private void Awake()
    {
        _hud.Initialize(_commandCenter);

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