using UnityEngine.SceneManagement;

namespace Assets._Asteroids.Logic.Services
{
    public class SceneLoader
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}