using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseState: IState
{
    private int _commandCenterCost;
    private ResourceMap _resourceMap;
    private Queue<Unit> _bindedUnits;
    private CommandCenter _commandCenter;
    private InputController _inputController;
    private ResourceScanner _resourceScanner;
    private BuildService _buildService;
    private BuildingView _buildingView;

    public BaseState(int commandCenterCost, ResourceMap resourceMap, Queue<Unit> bindedUnits, CommandCenter commandCenter, InputController inputController, ResourceScanner resourceScanner, BuildService buildService, BuildingView buildingView)
    {
        _commandCenterCost = commandCenterCost;
        _resourceMap = resourceMap;
        _bindedUnits = bindedUnits;
        _commandCenter = commandCenter;
        _inputController = inputController;
        _resourceScanner = resourceScanner;
        _buildService = buildService;
        _buildingView = buildingView;
    }

    private bool HasAvailableUnit => _bindedUnits.Count > 0;
    private bool HasHarvestableResource => _resourceMap.HasResources;
    private bool CanHarvestResource => HasAvailableUnit && HasHarvestableResource;


    public void OnSelect()
    {
        _inputController.ScanPressed += OnScanPressed;
        _inputController.BuildPressed += OnBuildPressed;
    }

    public void OnDeselect()
    {
        _inputController.ScanPressed -= OnScanPressed;
        _inputController.BuildPressed -= OnBuildPressed;
    }

    protected abstract void BuildCommandCenter(Vector3 position, Quaternion rotation);

    private void OnScanPressed()
    {
        _resourceScanner.Scan();
    }

    private void OnBuildPressed()
    {
        if (_buildService.IsAvaliable == false)
            _buildService.StartBuild(_buildingView, BuildCommandCenter);
    }

    private void TryHarvestResource()
    {
        if (CanHarvestResource == false)
            return;

        Unit unit = _bindedUnits.Dequeue();
        Resource targetResource = _resourceMap.GetResource();

        targetResource.Reserve();
        unit.HarvestResource(_commandCenter, targetResource);
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
        TryHarvestResource();
    }
}
