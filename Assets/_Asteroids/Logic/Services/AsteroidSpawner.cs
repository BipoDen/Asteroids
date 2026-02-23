using System;
using Assets._Asteroids.Logic.Factory;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets._Asteroids.Logic.Services
{
    public class AsteroidSpawner : ITickable
    {
        private float _spawnDelay = 3f;
        private float _asteroidSpeed = 2f;
        private int _fragmentCount = 3;
        private int _scorePerKill = 100;
        private bool _isSpawning;
        
        private AsteroidFactory _factory;
        private GameState _gameState;
        
        [Inject]
        public void Construct(AsteroidFactory factory, GameState gameState)
        {
            _factory = factory;
            _gameState = gameState;
        }

        public void Initialize()
        {
            SpawnAsteroid();
        }
        
        public void Tick()
        {
            if(_gameState.IsGamePaused)
                return;

            if (!_isSpawning)
                SpawnAsteroid();
        }
        
        private async UniTask SpawnAsteroid()
        {
            _isSpawning = true;
            _factory.Create(_asteroidSpeed, _fragmentCount, _scorePerKill);
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay));
            _isSpawning = false;
        }
    }
}