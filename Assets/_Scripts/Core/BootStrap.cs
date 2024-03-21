using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class BootStrap : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private Transform _commandCenterPoint;

    private CommandCenterSpawner _commandCenterSpawner;

    [Inject]
    private void Construct(CommandCenterSpawner commandCenterSpawner)
    {
        _commandCenterSpawner = commandCenterSpawner;       
    }

    private void Awake()
    {
        CommandCenter commandCenter = _commandCenterSpawner.Spawn(_commandCenterPoint.position, Quaternion.identity);

        foreach (Unit unit in _units)
        {
            commandCenter.BindUnit(unit);
        }
    }

    private void OnValidate()
    {
        _units = FindObjectsByType<Unit>(sortMode: FindObjectsSortMode.None).ToList();
    }
}