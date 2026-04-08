using Assets._Asteroids.Logic.Audio;
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
        private IVFXService _vfxService;
        private IAudioService _audioService;

        public AsteroidFactory(EnemyPool<AsteroidEnemy> pool, SpaceScreen spaceScreen, EnemyRepository repository, GameState gameState, 
            IVFXService vfxService, IAudioService audioService)
        {
            _pool = pool;
            _spaceScreen = spaceScreen;
            _repository = repository;
            _gameState = gameState;
            _vfxService = vfxService;
            _audioService = audioService;
        }

        public AsteroidEnemy Create(float speed, float fragmentSpeed, int fragmentsCount, int score, int fragmentScore, float baseSize, float fragmentSize)
        {
            var asteroid = _pool.Spawn();
            var spawnPosition = _spaceScreen.GetRandomSpawnPosition();
            var direction = _spaceScreen.GetRandomDirection(spawnPosition);
            asteroid.Initialize(speed, baseSize, direction, score);
            asteroid.transform.position = spawnPosition;
            
            asteroid.OnDied += Despawn;
            _repository.RegisterEnemy(asteroid);
            
            void Despawn()
            {
                CreateFragments(asteroid.transform, fragmentSpeed, fragmentsCount, fragmentScore, fragmentSize);
                _vfxService.CreateExplosion(asteroid.transform);
                _audioService.PlayExplosionAudio(.3f);
                _repository.UnregisterEnemy(asteroid);
                _pool.Despawn(asteroid);
            }    
                
            return asteroid;
        }

        public void CreateFragments(Transform parentTransform, float speed, int fragmentsCount, int score, float fragmentSize)
        {
            if(_gameState.IsGamePaused)
                return;
            
            for (int i = 0; i < fragmentsCount; i++)
            {
                var fragment =  _pool.Spawn();
                fragment.transform.position = parentTransform.position;
                var direction = _spaceScreen.GetRandomFragmentDirection();
                fragment.Initialize(speed, fragmentSize,  direction, score);
                
                fragment.OnDied += Despawn;
                _repository.RegisterEnemy(fragment);
                
                void Despawn()
                {
                    _vfxService.CreateExplosion(fragment.transform);
                    _audioService.PlayExplosionAudio(.2f);
                    _repository.UnregisterEnemy(fragment);
                    _pool.Despawn(fragment);
                }
            }
        }
    }
}