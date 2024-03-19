using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CommandCenter : MonoBehaviour, ICommandCenterNotifier
{
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField, Min(1)] private int _unitCost = 3;
    [SerializeField, Min(1)] private int _commandCenterCost = 5;
    [SerializeField] private BuildService _buildService;

    private int _resourceAmount = 0;

    private Queue<Unit> _units;
    private ResourceMap _resourceMap;
    private UnitFactory _unitFactory;
    private InputController _inputController;

    public event Action<int> ResourceAmountChanged;

    private bool HasAvailableUnit => _units.Count > 0;
    private bool HasHarvestableResource => _resourceMap.HasResources;
    private bool CanHarvestResource => HasAvailableUnit && HasHarvestableResource;

    [Inject]
    private void Construct(InputController inputController, UnitFactory unitFactory)
    {
        _inputController = inputController;
        _unitFactory = unitFactory;

        
    }

    private void Awake()
    {
        _units = new Queue<Unit>();
        _resourceMap = new ResourceMap();
    }

    private void OnEnable()
    {
        _inputController.ScanPressed += OnScanPressed;
        _inputController.BuildPressed += OnBuildPressed;
    }

    private void OnBuildPressed()
    {
        Debug.Log("Pressed");

        if(_buildService.IsActive == false)
        _buildService.StartBuild();
    }

    private void OnDisable()
    {
        _inputController.ScanPressed -= OnScanPressed;
        _inputController.BuildPressed -= OnBuildPressed;
    }

    public void Initialize()
    {
        _resourceScanner.Initialize(_resourceMap);
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

    public void BindUnit(Unit unit)
    {
        _units.Enqueue(unit);
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

        Unit unit = _units.Dequeue();
        Resource targetResource = _resourceMap.GetResource();

        targetResource.Reserve();
        unit.HarvestResource(this, targetResource);
    }

    private void OnScanPressed()
    {
        _resourceScanner.Scan();
    }

    private void TryCreateUnit()
    {
        if (_resourceAmount >= _unitCost)
        {
            Unit unit = _unitFactory.Create(transform.position);
            BindUnit(unit);
            SpendResources(_unitCost);
        }
    }

    private void SpendResources (int amount)
    {
        _resourceAmount -= amount;
        ResourceAmountChanged?.Invoke(_resourceAmount);
    }
}
