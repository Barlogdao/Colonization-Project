using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private UnitFactory _unitFactory;

    [SerializeField] private CommandCenter _commandCenterPrefab;
    [SerializeField] private BuildService _buildService;

    public override void InstallBindings()
    {
        BindInput();
        Container.BindInterfacesAndSelfTo<Selector>().AsSingle();

        Container.Bind<UnitFactory>().FromInstance(_unitFactory).AsSingle();

        BindCommandCenter();
    }

    private void BindInput()
    {
        Container.BindInterfacesAndSelfTo<InputController>().AsSingle().NonLazy();
    }

    private void BindCommandCenter()
    {
        Container.Bind<BuildService>().FromInstance(_buildService).AsSingle();
        Container.BindInterfacesAndSelfTo<CommandCenterSpawner>().AsSingle();
        Container.BindFactory<CommandCenter, CommandCenter.Factory>().FromComponentInNewPrefab(_commandCenterPrefab);
    }
}