using System;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private BuildService _buildServicePrefab;
    [SerializeField] private UnitFactory _unitFactory;
    [SerializeField] private CommandCenter _commandCenterPrefab;

    public override void InstallBindings()
    {
        BindInput();
        BindSelector();
        BindBuildService();

        Container.Bind<UnitFactory>().FromInstance(_unitFactory).AsSingle();

        BindCommandCenter();
    }

    private void BindInput()
    {
        Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
    }

    private void BindSelector()
    {
        Container.BindInterfacesAndSelfTo<Selector>().AsSingle();
    }

    private void BindBuildService()
    {
        Container.Bind<BuildService>().FromComponentInNewPrefab(_buildServicePrefab).AsSingle();
    }

    private void BindCommandCenter()
    {
        Container.BindInterfacesAndSelfTo<CommandCenterSpawner>().AsSingle();
        Container.BindFactory<CommandCenter, CommandCenter.Factory>().FromComponentInNewPrefab(_commandCenterPrefab);
    }
}