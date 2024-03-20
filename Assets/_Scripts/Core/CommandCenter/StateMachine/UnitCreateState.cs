using System.Collections.Generic;
using UnityEngine;

public class UnitCreateState: BaseState
{
    private int _unitCost;
    private ResourceStorage _resourceStorage;
    private UnitFactory _unitFactory;
    private CommandCenter _commandCenter;

    public UnitCreateState(int commandCenterCost, ResourceMap resourceMap, Queue<Unit> bindedUnits, CommandCenter commandCenter, InputController inputController, ResourceScanner resourceScanner, BuildService buildService, BuildingView buildingView) : base(commandCenterCost, resourceMap, bindedUnits, commandCenter, inputController, resourceScanner, buildService, buildingView)
    {
    }

    //public UnitCreateState(int unitCost, ResourceStorage resourceStorage, UnitFactory unitFactory, CommandCenter commandCenter)
    //{
    //    _unitCost = unitCost;
    //    _resourceStorage = resourceStorage;
    //    _unitFactory = unitFactory;
    //    _commandCenter = commandCenter;
    //}

    public override void Enter()
    {

    }

    public override void Update()
    {
        if (_resourceStorage.TrySpend(_unitCost) == true)
        {
            Unit unit = _unitFactory.Create(_commandCenter.transform.position);
            _commandCenter.BindUnit(unit);
        }
    }

    public override void Exit()
    {

    }

    protected override void BuildCommandCenter(Vector3 position, Quaternion rotation)
    {
        
    }
}