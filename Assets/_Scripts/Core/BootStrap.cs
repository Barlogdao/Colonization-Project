using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BootStrap : MonoBehaviour
{
    [SerializeField] private CommandCenter _commandCenter;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private HUD _hud;
    [SerializeField] Selector _selector;

    private void Awake()
    {
        _hud.Initialize(_selector);

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