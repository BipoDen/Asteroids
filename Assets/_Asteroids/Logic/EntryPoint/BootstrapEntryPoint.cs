using System;
using Assets._Asteroids.Logic.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class BootstrapEntryPoint : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(GameplayConstants.GAMEPLEY_SCENE_NAME);
        }
    }
}