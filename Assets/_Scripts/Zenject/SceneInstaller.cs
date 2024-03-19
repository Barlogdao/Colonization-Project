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
        Container.Bind<UnitFactory>().FromInstance(_unitFactory).AsSingle();

        Container.Bind<BuildService>().FromInstance(_buildService).AsSingle();
        Container.BindInterfacesAndSelfTo<CommandCenterSpawner>().AsSingle();
        Container.BindFactory<CommandCenter, CommandCenter.Factory>().FromComponentInNewPrefab(_commandCenterPrefab);
    }

    private void BindInput()
    {
        Container.BindInterfacesAndSelfTo<InputController>().AsSingle().NonLazy();
    }
}