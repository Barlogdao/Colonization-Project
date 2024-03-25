using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CommandCenter : MonoBehaviour, ICommandCenterNotifier, ISelectable
{
    [SerializeField] private BuildingView _view;
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private CommandCenterBuilderModule _commandCenterBuilder;
    [SerializeField] private UnitCreatorModule _unitCreatorModule;
    [SerializeField] private Flag _flag;
    [SerializeField] private Collider _collider;

    private InputController _inputController;
    private ResourceMap _resourceMap;
    private ResourceStorage _resourceStorage;
    private Queue<Unit> _units;

    public event Action<int> ResourceAmountChanged;

    public Vector3 Position => transform.position;
    public float ScannerCooldownProgress => _resourceScanner.CooldownProgress;

    private bool HasAvailableUnit => _units.Count > 0;
    private bool HasHarvestableResource => _resourceMap.HasResources;
    private bool CanHarvestResource => HasAvailableUnit && HasHarvestableResource;

    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;

        _resourceStorage = new ResourceStorage();
        _units = new Queue<Unit>();
        _resourceMap = new ResourceMap();

        _resourceScanner.Initialize(_resourceMap);
        _commandCenterBuilder.Initialize(_flag, _resourceStorage, _view, _units);
        _unitCreatorModule.Initialize(this, _flag, _resourceStorage);
    }

    private void OnEnable()
    {
        _resourceStorage.AmountChanged += OnResourceAmountChanged;
    }

    private void OnDisable()
    {
        _resourceStorage.AmountChanged -= OnResourceAmountChanged;
    }

    private void Update()
    {
        TryHarvestResource();
    }

    private void Start()
    {
        _view.ShowSpawn();
    }

    public void BindUnit(Unit unit)
    {
        _units.Enqueue(unit);
    }

    public Vector3 GetClosestPoint(Transform transform)
    {
        return _collider.ClosestPoint(transform.position);
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

        Unit unit = _units.Dequeue();
        Resource targetResource = _resourceMap.GetResource();

        targetResource.Reserve();
        unit.HarvestResource(this, targetResource);
    }

    private void OnResourceAmountChanged(int amount) => ResourceAmountChanged?.Invoke(amount);
    private void OnScanPressed() => _resourceScanner.Scan();
    private void OnBuildPressed() => _commandCenterBuilder.EnterBuildMode();

    public class Factory : PlaceholderFactory<CommandCenter> { }
}