using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Repository;
using Assets._Asteroids.Logic.Services;
using UnityEngine;

namespace Assets._Asteroids.Logic.Factory
{
    public class AsteroidFactory
    {
        private EnemyPool<AsteroidEnemy> _pool;
        private SpaceScreen _spaceScreen;
        private EnemyRepository _repository;
        private GameState _gameState;

        public AsteroidFactory(EnemyPool<AsteroidEnemy> pool, SpaceScreen spaceScreen, EnemyRepository repository, GameState gameState)
        {
            _pool = pool;
            _spaceScreen = spaceScreen;
            _repository = repository;
            _gameState = gameState;
        }

        public AsteroidEnemy Create(float speed, int fragmentsCount, int score)
        {
            var asteroid = _pool.Spawn();
            var spawnPosition = _spaceScreen.GetRandomSpawnPosition();
            var direction = _spaceScreen.GetRandomDirection(spawnPosition);
            asteroid.Initialize(speed, 1, direction, score);
            asteroid.transform.position = spawnPosition;
            
            asteroid.OnDied += Despawn;
            _repository.RegisterEnemy(asteroid);
            
            void Despawn()
            {
                CreateFragments(asteroid.transform, speed, fragmentsCount, score/2);
                _repository.UnregisterEnemy(asteroid);
                _pool.Despawn(asteroid);
            }    
                
            return asteroid;
        }

        public void CreateFragments(Transform parentTransform, float speed, int fragmentsCount, int score)
        {
            if(_gameState.IsGamePaused)
                return;
            
            for (int i = 0; i < fragmentsCount; i++)
            {
                var fragment =  _pool.Spawn();
                fragment.transform.position = parentTransform.position;
                var direction = _spaceScreen.GetRandomFragmentDirection();
                fragment.Initialize(speed * 1.5f, .3f,  direction, score);
                
                fragment.OnDied += Despawn;
                _repository.RegisterEnemy(fragment);
                
                void Despawn()
                {
                    _repository.UnregisterEnemy(fragment);
                    _pool.Despawn(fragment);
                }
            }
        }
    }
}