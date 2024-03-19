using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private UnitFactory _unitFactory;

    public override void InstallBindings()
    {
        BindInput();
        Container.Bind<UnitFactory>().FromInstance(_unitFactory).AsSingle();
    }

    private void BindInput()
    {
        Container.BindInterfacesAndSelfTo<InputController>().AsSingle().NonLazy();
    }
}