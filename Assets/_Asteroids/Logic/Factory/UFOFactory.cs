using Assets._Asteroids.Logic.Audio;
using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Repository;
using Assets._Asteroids.Logic.Services;
using UnityEngine;

namespace Assets._Asteroids.Logic.Factory
{
    public class UFOFactory
    {
        private EnemyPool<UFOEnemy> _pool;
        private SpaceScreen _spaceScreen;
        private EnemyRepository _repository;
        private IVFXService _vfxService;
        private IAudioService _audioService;

        public UFOFactory(EnemyPool<UFOEnemy> pool, SpaceScreen spaceScreen, EnemyRepository repository, IVFXService vfxService, IAudioService audioService)
        {
            _pool = pool;
            _spaceScreen = spaceScreen;
            _repository = repository;
            _vfxService = vfxService;
            _audioService = audioService;
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
                _vfxService.CreateExplosion(ufoEnemy.transform);
                _audioService.PlayExplosionAudio(.3f);
                _repository.UnregisterEnemy(ufoEnemy);
                _pool.Despawn(ufoEnemy);
            }    
                
            return ufoEnemy;
        }
    }
}