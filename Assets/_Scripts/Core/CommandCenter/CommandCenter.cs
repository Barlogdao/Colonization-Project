using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommandCenter : MonoBehaviour, ICommandCenterNotifier
{
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private float _scanCooldown;

    private int _resourceAmount = 0;

    private Queue<Unit> _units;
    private ResourceMap _resourceMap;
    private PlayerInput _playerInput;
    private CooldownTimer _scannerCooldown;

    public event Action<int> ResourceAmountChanged;

    private bool HasAvailableUnit => _units.Count > 0;
    private bool HasHarvestableResource => _resourceMap.HasResources;
    private bool CanHarvestResource => HasAvailableUnit && HasHarvestableResource;

    public void Initialize(PlayerInput playerInput)
    {
        _units = new Queue<Unit>();
        _resourceMap = new ResourceMap();
        _playerInput = playerInput;
        _resourceScanner.Initialize(_resourceMap);
        _scannerCooldown = new CooldownTimer(_scanCooldown);

        _playerInput.Game.Scan.performed += OnScanPressed;
    }

    private void Start()
    {
        ResourceAmountChanged?.Invoke(_resourceAmount);
    }

    private void Update()
    {
        _scannerCooldown.Update();
        TryHarvestResource();
    }

    private void OnDestroy()
    {
        _playerInput.Game.Scan.performed -= OnScanPressed;
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

    private void OnScanPressed(InputAction.CallbackContext context)
    {
        if (_scannerCooldown.IsReady == true)
        {
            Scan();
            _scannerCooldown.Reset();
        }
    }

    private void Scan()
    {
        _resourceScanner.Scan();
    }
}
