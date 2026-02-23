using System;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Factory;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets._Asteroids.Logic.Services
{
    public class UFOSpawner : ITickable
    {
        private float _spawnDelay = 6f;
        private float _UFOSpeed = 2f;
        private int _scorePerKill = 200;
        private bool _isSpawning;
        
        private SpaceshipController _player;
        private UFOFactory _factory;
        private GameState _gameState;

        public UFOSpawner(UFOFactory factory, SpaceshipController player, GameState gameState)
        {
            _factory = factory;
            _player = player;
            _gameState = gameState;
        }

        public void Initialize()
        {
            
        }
        public void Tick()
        {
            if(_gameState.IsGamePaused)
                return;

            if (!_isSpawning)
                SpawnUFO();
        }
        
        private async UniTask SpawnUFO()
        {
            _isSpawning = true;
            _factory.Create(_player.transform, _UFOSpeed, _scorePerKill);
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay));
            _isSpawning = false;
        }
        
    }
}