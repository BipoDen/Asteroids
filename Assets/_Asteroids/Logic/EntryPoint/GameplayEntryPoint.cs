using Assets._Asteroids.Logic.Addressable;
using Assets._Asteroids.Logic.Constants;
using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class GameplayEntryPoint : IInitializable
    {
        private SpaceshipController _player;
        private ScoreService _scoreService;
        private GameState _gameState;
        private IAssetLoader _assetLoader;
        private EnemyPool<AsteroidEnemy> _asteroidPool;
        private EnemyPool<UFOEnemy> _ufoPool;
        private AsteroidSpawner _asteroidSpawner;
        private UFOSpawner _ufoSpawner;
        
        public GameplayEntryPoint( 
            ScoreService scoreService, 
            GameState gameState,
            SpaceshipController player, 
            IAssetLoader assetLoader,
            EnemyPool<UFOEnemy> ufoPool, 
            EnemyPool<AsteroidEnemy> asteroidPool, 
            UFOSpawner ufoSpawner, 
            AsteroidSpawner asteroidSpawner)
        {
            _scoreService = scoreService;
            _gameState = gameState;
            _player = player;
            _ufoPool = ufoPool;
            _asteroidPool = asteroidPool;
            _ufoSpawner = ufoSpawner;
            _asteroidSpawner = asteroidSpawner;
            _assetLoader = assetLoader;
        }
        public void Initialize()
        {
            _scoreService.Initialize();
            _gameState.Initialize(_player);

            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            var asteroidPrefab = await _assetLoader.LoadAsync<GameObject>(AddressablesConstants.ASTEROIDS_ID);
            _asteroidPool.Initialize(asteroidPrefab, 10);
            _asteroidSpawner.StartWork();
            
            var ufoPrefab = await _assetLoader.LoadAsync<GameObject>(AddressablesConstants.UFO_ID);
            _ufoPool.Initialize(ufoPrefab, 10);
            _ufoSpawner.StartWork();
        }
        
    }
}