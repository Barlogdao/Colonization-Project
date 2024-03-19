using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CommandCenter : MonoBehaviour, ICommandCenterNotifier
{
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField, Min(1)] private int _unitCost = 3;
    [SerializeField, Min(1)] private int _commandCenterCost = 5;
    [SerializeField] private BuildingView _view;
    [SerializeField] private Flag _flagPrefab;

    public BuildingView BuildingView => _view;

    private int _resourceAmount = 5;

    private Queue<Unit> _bindedUnits;
    private ResourceMap _resourceMap;
    private UnitFactory _unitFactory;
    private CommandCenterSpawner _commandCenterSpawner;
    private InputController _inputController;
    private BuildService _buildService;

    private bool _isBuildinginQueue = false;
    private Coroutine _buildRoutine;
    private Flag _placedFlag;


    public event Action<int> ResourceAmountChanged;

    private bool HasAvailableUnit => _bindedUnits.Count > 0;
    private bool HasHarvestableResource => _resourceMap.HasResources;
    private bool CanHarvestResource => HasAvailableUnit && HasHarvestableResource;

    [Inject]
    private void Construct(InputController inputController, UnitFactory unitFactory, CommandCenterSpawner commandCenterSpawner, BuildService buildService)
    {
        _inputController = inputController;
        _unitFactory = unitFactory;
        _commandCenterSpawner = commandCenterSpawner;
        _buildService = buildService;
    }

    private void Awake()
    {
        _bindedUnits = new Queue<Unit>();
        _resourceMap = new ResourceMap();
        _resourceScanner.Initialize(_resourceMap);
    }

    private void OnEnable()
    {
        _inputController.ScanPressed += OnScanPressed;
        _inputController.BuildPressed += OnBuildPressed;
    }

    private void Start()
    {
        ResourceAmountChanged?.Invoke(_resourceAmount);
    }

    private void Update()
    {
        TryCreateUnit();
        TryHarvestResource();
    }

    private void OnDisable()
    {
        _inputController.ScanPressed -= OnScanPressed;
        _inputController.BuildPressed -= OnBuildPressed;
    }

    public void BindUnit(Unit unit)
    {
        _bindedUnits.Enqueue(unit);
    }

    public void AcceptResource(Resource resource)
    {
        _resourceAmount += resource.Amount;
        ResourceAmountChanged?.Invoke(_resourceAmount);
        resource.Remove();
    }

    private void TryHarvestResource()
    {
        if (CanHarvestResource == false)
            return;

        Unit unit = _bindedUnits.Dequeue();
        Resource targetResource = _resourceMap.GetResource();

        targetResource.Reserve();
        unit.HarvestResource(this, targetResource);
    }

    private void OnScanPressed()
    {
        _resourceScanner.Scan();
    }

    private void OnBuildPressed()
    {
        if (_buildService.IsActive == false)
            _buildService.StartBuild(BuildCommandCenter);
    }

    private void BuildCommandCenter(Vector3 position, Quaternion rotation)
    {
        if (_buildRoutine != null)
        {
            StopCoroutine(_buildRoutine);
        }

        _buildRoutine = StartCoroutine(BuildRoutine(position, rotation));
    }

    private IEnumerator BuildRoutine(Vector3 position, Quaternion rotation)
    {
        _isBuildinginQueue = true;

        if (_placedFlag == null)
        {
            _placedFlag = Instantiate(_flagPrefab, position, rotation);
        }
        else
        {
            _placedFlag.transform.position = position;
            _placedFlag.transform.rotation = rotation;
        }

        yield return new WaitUntil(() => _resourceAmount >= _commandCenterCost && HasAvailableUnit);

        Unit unit = _bindedUnits.Dequeue();
        unit.BuildCommandCenter(_placedFlag, _commandCenterSpawner);
        _isBuildinginQueue = false;
    }

    private void TryCreateUnit()
    {
        return;

        if (_isBuildinginQueue)
            return;

        if (_resourceAmount >= _unitCost)
        {
            Unit unit = _unitFactory.Create(transform.position);
            BindUnit(unit);
            SpendResources(_unitCost);
        }
    }

    private void SpendResources(int amount)
    {
        _resourceAmount -= amount;
        ResourceAmountChanged?.Invoke(_resourceAmount);
    }

    public class Factory : PlaceholderFactory<CommandCenter> { }
}
