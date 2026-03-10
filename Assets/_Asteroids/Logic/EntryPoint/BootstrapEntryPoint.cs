using Assets._Asteroids.Logic.Gameplay;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class BootstrapEntryPoint : IInitializable
    {
        public void Initialize()
        {
            SceneManager.LoadScene(GameplayConstants.GAMEPLEY_SCENE_NAME);
        }
    }
}