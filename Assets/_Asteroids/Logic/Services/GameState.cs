using System;
using Assets._Asteroids.Logic.Ads;
using Assets._Asteroids.Logic.Analytics;
using Assets._Asteroids.Logic.Entities.Player;
using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Services
{
    public class GameState : IDisposable
    {
        private SpaceshipController _player;
        private StatsService _stats;
        private IAnalyticsService _analytics;
        private IAdService _adService;
        public event Action OnGameOver;
        public event Action OnGameRestart;
        
        public bool IsGamePaused { get; private set; }

        public GameState(StatsService statsService, IAnalyticsService analytics, IAdService adService)
        {
            _stats = statsService;
            _analytics = analytics;
            _adService = adService;
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

        public void ContinueGame()
        {
            IsGamePaused = false;
        }

        private void GameOver()
        {
            IsGamePaused = true;
            OnGameOver?.Invoke();
        }

        public async UniTask RestartGame()
        {
            _analytics.OnGameOverEvent(_stats.PrimaryCount, _stats.SecondaryCount, _stats.AsteroidsKillCount, _stats.UFOKillCount);
            await _adService.ShowInterstitialAd();
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