using UnityEngine;
using Zenject;

public class UnitCreatorModule : MonoBehaviour
{
    [SerializeField, Min(1)] private int _unitCost = 3;

    private UnitFactory _unitFactory;

    private CommandCenter _commandCenter;
    private Flag _flag;
    private ResourceStorage _resourceStorage;

    [Inject]
    private void Construct(UnitFactory unitFactory)
    {
        _unitFactory = unitFactory;
    }

    public void Initialize(CommandCenter commandCenter, Flag flag, ResourceStorage resourceStorage)
    {
        _commandCenter = commandCenter;
        _flag = flag;
        _resourceStorage = resourceStorage;
    }

    private void Update()
    {
        if (_flag.IsSet == true)
            return;

        if (_resourceStorage.TrySpend(_unitCost) == true)
        {
            Unit unit = _unitFactory.Create(transform.position);
            _commandCenter.BindUnit(unit);
        }
    }
}