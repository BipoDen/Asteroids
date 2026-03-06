using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
        }

        private void BindServices()
        {
            Container.Bind<ISaveService>().To<SaveService>().FromNew().AsSingle();
            var saveService = Container.Resolve<ISaveService>();
            var data = saveService.Load();
            Container.Bind<SaveData>().FromInstance(data).AsSingle();
        }
    }
}