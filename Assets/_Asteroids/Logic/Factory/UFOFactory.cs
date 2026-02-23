using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Repository;
using Assets._Asteroids.Logic.Services;
using UnityEngine;

namespace Assets._Asteroids.Logic.Factory
{
    public class UFOFactory
    {
        private UFOPool _pool;
        private SpaceScreen _spaceScreen;
        private EnemyRepository _repository;

        public UFOFactory(UFOPool pool, SpaceScreen spaceScreen, EnemyRepository repository)
        {
            _pool = pool;
            _spaceScreen = spaceScreen;
            _repository = repository;
        }

        public UFOEnemy Create(Transform playerTarget, float speed, int score)
        {
            var ufoEnemy = _pool.Spawn();
            var spawnPosition = _spaceScreen.GetRandomSpawnPosition();
            _repository.RegisterEnemy(ufoEnemy);
            ufoEnemy.Init(playerTarget, speed, score);
            ufoEnemy.transform.position = spawnPosition;
            
            ufoEnemy.OnDied += Despawn;
            
            void Despawn()
            {
                _repository.UnregisterEnemy(ufoEnemy);
                _pool.Despawn(ufoEnemy);
            }    
                
            return ufoEnemy;
        }
    }
}