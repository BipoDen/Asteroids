using System;
using Assets._Asteroids.Logic.Analytics;
using Assets._Asteroids.Logic.Entities.Player;

namespace Assets._Asteroids.Logic.Services
{
    public class GameState : IDisposable
    {
        private SpaceshipController _player;
        private StatsService _stats;
        private IAnalyticsService _analytics;
        public event Action OnGameOver;
        public event Action OnGameRestart;
        
        public bool IsGamePaused { get; private set; }

        public GameState(StatsService statsService, IAnalyticsService analytics)
        {
            _stats = statsService;
            _analytics = analytics;
        }

        public void Initialize(SpaceshipController player)
        {
            _player = player;
            _player.OnGameOver += GameOver;
            GameStart();
        }

        public void GameStart()
        {
            IsGamePaused = false;
            _analytics.OnStartGameEvent();
        }

        public void PauseGame()
        {
            IsGamePaused = true;
        }

        private void GameOver()
        {
            IsGamePaused = true;
            OnGameOver?.Invoke();
            _analytics.OnGameOverEvent(_stats.PrimaryCount, _stats.SecondaryCount, _stats.AsteroidsKillCount, _stats.UFOKillCount);
        }

        public void RestartGame()
        {
            GameStart();
            _player.ResetPosition();
            OnGameRestart?.Invoke();
        }

        public void Dispose()
        {
            _player.OnGameOver -= GameOver;
        }
    }
}