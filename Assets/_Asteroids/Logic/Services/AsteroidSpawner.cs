using System;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.RemoteConfig;
using Assets._Asteroids.Logic.RemoteConfig.Configs;
using Assets._Asteroids.Logic.RemoteConfig.Configs.Enemies;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Services
{
    public class AsteroidSpawner : ITickable
    {
        private AsteroidsConfig _config;
        private bool _isSpawning;
        
        private AsteroidFactory _factory;
        private GameState _gameState;
        private IRemoteConfig _configProvider;
        private bool _isReady;

        [Inject]
        public void Construct(AsteroidFactory factory, GameState gameState, IRemoteConfig configProvider)
        {
            _factory = factory;
            _gameState = gameState;
            _configProvider = configProvider;
        }

        public void Initialize()
        {
            _isReady = true;
            _config = _configProvider.GetRemoteConfig<AsteroidsConfig>();
        }
        
        public void Tick()
        {
            if(_gameState.IsGamePaused || !_isReady)
                return;

            if (!_isSpawning)
                SpawnAsteroid();
        }
        
        private async UniTask SpawnAsteroid()
        {
            _isSpawning = true;
            _factory.Create(_config.AsteroidSpeed, 
                _config.FragmentSpeed, 
                _config.FragmentCount, 
                _config.ScorePerKill,
                _config.ScorePerFragmentKill,
                _config.AsteroidSize,
                _config.FragmentSize);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.SpawnDelay));
            _isSpawning = false;
        }
    }
}