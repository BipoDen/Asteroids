using Assets._Asteroids.Logic.Analytics;
using Assets._Asteroids.Logic.Analytics.Firebase;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using Zenject;

namespace Assets._Asteroids.Logic.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveService>().To<SaveService>().FromNew().AsSingle();
            var saveService = Container.Resolve<ISaveService>();
            var data = saveService.Load();
            Container.Bind<SaveData>().FromInstance(data).AsSingle();
            
            Container.BindInterfacesTo<FirebaseInitializer>().FromNew().AsSingle();
            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsService>().AsSingle();
        }
    }
}