using System;
using Assets._Asteroids.Logic.Ads;
using Assets._Asteroids.Logic.Analytics;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Gameplay;
using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Services
{
    public class GameState : IDisposable
    {
        private SpaceshipController _player;
        private StatsService _stats;
        private IAnalyticsService _analytics;
        private IAdService _adService;
        private SaveData _saveData;
        public event Action OnGameOver;
        public event Action OnGameRestart;
        
        public bool IsGamePaused { get; private set; }

        public GameState(StatsService statsService, IAnalyticsService analytics, IAdService adService, SaveData saveData)
        {
            _stats = statsService;
            _analytics = analytics;
            _adService = adService;
            _saveData = saveData;
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
            if(!_saveData.IsAdDisabled)
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