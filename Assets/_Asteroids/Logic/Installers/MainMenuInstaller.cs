using Assets._Asteroids.Logic.EntryPoint;
using Assets._Asteroids.Logic.UI;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _canvas;
        public override void InstallBindings()
        {
            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();
            Container.Bind<MainMenuUIPresenter>().AsSingle();
            
            Container.BindInterfacesTo<MainMenuEntryPoint>().AsSingle();
        }
    }
}