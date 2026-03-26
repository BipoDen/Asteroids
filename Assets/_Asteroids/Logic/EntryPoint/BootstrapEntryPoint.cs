using Assets._Asteroids.Logic.Constants;
using Assets._Asteroids.Logic.RemoteConfig;
using Assets._Asteroids.Logic.Services;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class BootstrapEntryPoint : IInitializable
    {
        private IRemoteConfig _remoteConfig;
        private SceneLoader _sceneLoader;

        public BootstrapEntryPoint(IRemoteConfig remoteConfig, SceneLoader sceneLoader)
        {
            _remoteConfig = remoteConfig;
            _sceneLoader = sceneLoader;
        }

        public async void Initialize()
        {
            await _remoteConfig.Initialize();
            _sceneLoader.LoadScene(GameplayConstants.MAIN_MENU_SCENE_NAME);
        }
    }
}