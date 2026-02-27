using System;
using Assets._Asteroids.Logic.Entities.Player;
using UnityEngine;

namespace Assets._Asteroids.Logic.Services
{
    public class GameState : IDisposable
    {
        private SpaceshipController _player;
        public event Action OnGameOver;
        public event Action OnGameRestart;
        
        public bool IsGamePaused { get; private set; }

        public void Initialize(SpaceshipController player)
        {
            _player = player;
            _player.OnGameOver += GameOver;
        }

        public void GameStart()
        {
            IsGamePaused = false;
        }

        public void PauseGame()
        {
            IsGamePaused = true;
        }

        private void GameOver()
        {
            IsGamePaused = true;
            OnGameOver?.Invoke();
        }

        public void RestartGame()
        {
            IsGamePaused = false;
            _player.ResetPosition();
            OnGameRestart?.Invoke();
        }

        public void Dispose()
        {
            _player.OnGameOver -= GameOver;
        }
    }
}