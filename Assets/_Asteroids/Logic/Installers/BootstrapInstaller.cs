using Assets._Asteroids.Logic.EntryPoint;
using Zenject;

namespace Assets._Asteroids.Logic.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BootstrapEntryPoint>().AsSingle();
        }
    }
}