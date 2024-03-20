using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CommandCenter : MonoBehaviour, ICommandCenterNotifier, ISelectable
{
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField, Min(1)] private int _unitCost = 3;
    [SerializeField, Min(1)] private int _commandCenterCost = 5;
    [SerializeField] private BuildingView _view;
    [SerializeField] private Flag _flagPrefab;

    private ResourceStorage _resourceStorage;

    private int _resourceAmount = 0;

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

    public Vector3 Position => transform.position;
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
        _resourceStorage = new ResourceStorage();
        _bindedUnits = new Queue<Unit>();
        _resourceMap = new ResourceMap();
        _resourceScanner.Initialize(_resourceMap);
    }

    private void OnEnable()
    {
        _resourceStorage.AmountChanged += OnResourceAmountChanged;
    }

    private void OnDisable()
    {
        _resourceStorage.AmountChanged -= OnResourceAmountChanged;
    }

    private void Start()
    {
        _view.ShowSpawn();
    }

    private void Update()
    {
        TryCreateUnit();
        TryHarvestResource();
    }

    public void BindUnit(Unit unit)
    {
        _bindedUnits.Enqueue(unit);
    }

    public void AcceptResource(Resource resource)
    {
        _resourceStorage.Add(resource.Amount);
        resource.Remove();
    }

    public void Select()
    {
        _view.ShowOutline();

        _inputController.ScanPressed += OnScanPressed;
        _inputController.BuildPressed += OnBuildPressed;

        ResourceAmountChanged?.Invoke(_resourceStorage.Amount);
    }

    public void Deselect()
    {
        _view.HideOutline();

        _inputController.ScanPressed -= OnScanPressed;
        _inputController.BuildPressed -= OnBuildPressed;
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

    private void OnResourceAmountChanged(int amount)
    {
        ResourceAmountChanged?.Invoke(amount);
    }

    private void OnScanPressed()
    {
        _resourceScanner.Scan();
    }

    private void OnBuildPressed()
    {
        if (_buildService.IsActive == false)
            _buildService.StartBuild(_view, BuildCommandCenter);
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

        yield return new WaitUntil(() => _resourceStorage.CanSpend(_commandCenterCost) && HasAvailableUnit);

        Unit unit = _bindedUnits.Dequeue();
        //unit.BuildCommandCenter(_placedFlag, _commandCenterSpawner);

        yield return unit.BuildRoutine(_placedFlag, _commandCenterSpawner);
        _resourceStorage.TrySpend(_commandCenterCost);
        _isBuildinginQueue = false;
    }

    private void TryCreateUnit()
    {
        if (_isBuildinginQueue)
            return;

        if (_resourceStorage.TrySpend(_unitCost) == true)
        {
            Unit unit = _unitFactory.Create(transform.position);
            BindUnit(unit);
        }
    }

    public class Factory : PlaceholderFactory<CommandCenter> { }
}
