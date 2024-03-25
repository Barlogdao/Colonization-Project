using UnityEngine;
using Zenject;

public class BootStrap : MonoBehaviour
{
    [SerializeField] private Transform _commandCenterPoint;
    [SerializeField] private Transform[] _unitPoints;

    private CommandCenterSpawner _commandCenterSpawner;
    private UnitSpawner _unitSpawner;

    [Inject]
    private void Construct(CommandCenterSpawner commandCenterSpawner, UnitSpawner unitSpawner)
    {
        _commandCenterSpawner = commandCenterSpawner;
        _unitSpawner = unitSpawner;

    }

    private void Awake()
    {
        CommandCenter commandCenter = _commandCenterSpawner.Spawn(_commandCenterPoint.position, Quaternion.identity);

        foreach (Transform point in _unitPoints)
        {
            Unit unit = _unitSpawner.Spawn(point.position);

            commandCenter.BindUnit(unit);
        }
    }
}