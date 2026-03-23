using System;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.RemoteConfig;
using Assets._Asteroids.Logic.RemoteConfig.Configs;
using Assets._Asteroids.Logic.RemoteConfig.Configs.Enemies;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets._Asteroids.Logic.Services
{
    public class UFOSpawner : ITickable
    {
        private UFOsConfig _config;
        private bool _isSpawning;
        
        private SpaceshipController _player;
        private UFOFactory _factory;
        private GameState _gameState;
        private IRemoteConfig _configProvider;
        private bool _isReady;

        public UFOSpawner(UFOFactory factory, GameState gameState, IRemoteConfig configProvider)
        {
            _factory = factory;
            _gameState = gameState;
            _configProvider = configProvider;
        }

        public void Initialize(SpaceshipController player)
        {
            _player = player;
            _isReady = true;
            _config = _configProvider.GetRemoteConfig<UFOsConfig>();
        }
        
        public void Tick()
        {
            if(_gameState.IsGamePaused || !_isReady)
                return;

            if (!_isSpawning)
                SpawnUFO();
        }
        
        private async UniTask SpawnUFO()
        {
            _isSpawning = true;
            _factory.Create(_player.transform, _config.UFOSpeed, _config.ScorePerKill);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.SpawnDelay));
            _isSpawning = false;
        }
        
    }
}