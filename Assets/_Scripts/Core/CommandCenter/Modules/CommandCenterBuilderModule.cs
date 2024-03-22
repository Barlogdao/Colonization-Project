using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CommandCenterBuilderModule : MonoBehaviour
{
    [SerializeField, Min(1)] private int _commandCenterCost = 5;

    private BuildService _buildService;
    private CommandCenterSpawner _commandCenterSpawner;

    private Flag _flag;
    private Queue<Unit> _units;
    private ResourceStorage _resourceStorage;
    private BuildingView _view;

    private Coroutine _buildRoutine;

    [Inject]
    private void Construct(BuildService buildService, CommandCenterSpawner commandCenterSpawner)
    {
        _buildService = buildService;
        _commandCenterSpawner = commandCenterSpawner;
    }

    public void Initialize(Flag flag, ResourceStorage resourceStorage, BuildingView view, Queue<Unit> units)
    {
        _flag = flag;
        _resourceStorage = resourceStorage;
        _view = view;
        _units = units;
    }

    public void EnterBuildMode()
    {
        if (_buildService.IsAvaliable == false)
            return;

        _buildService.ActivateBuildMode(_view, BuildCommandCenter);
    }

    private void BuildCommandCenter(Vector3 position, Quaternion rotation)
    {
        if (_flag.IsSet == true)
        {
            _flag.Set(position, rotation);
            return;
        }

        if (_buildRoutine != null)
            StopCoroutine(_buildRoutine);

        _buildRoutine = StartCoroutine(BuildRoutine(position, rotation));
    }

    private IEnumerator BuildRoutine(Vector3 position, Quaternion rotation)
    {
        _flag.Set(position, rotation);

        yield return new WaitUntil(() => _units.Count > 0 && _resourceStorage.TrySpend(_commandCenterCost) == true);

        Unit unit = _units.Dequeue();
        unit.BuildConstruction(_flag, SpawnCommandCenter);
    }

    private void SpawnCommandCenter(Unit unit)
    {
        CommandCenter commandCenter = _commandCenterSpawner.Spawn(_flag.transform.position, _flag.transform.rotation);
        commandCenter.BindUnit(unit);

        _flag.Unset();
    }
}
