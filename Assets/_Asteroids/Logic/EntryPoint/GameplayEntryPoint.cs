using Assets._Asteroids.Logic.Addressable;
using Assets._Asteroids.Logic.Constants;
using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.UI;
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
        private SpaceshipFactory _playerFactory;
        private GameplayUIModel _gameplayUIModel;
        private ProjectilePool _projectilePool;
        
        public GameplayEntryPoint( 
            ScoreService scoreService, 
            GameState gameState,
            IAssetLoader assetLoader,
            EnemyPool<UFOEnemy> ufoPool, 
            EnemyPool<AsteroidEnemy> asteroidPool, 
            UFOSpawner ufoSpawner, 
            AsteroidSpawner asteroidSpawner, 
            SpaceshipFactory playerFactory, 
            GameplayUIModel gameplayUIModel, 
            ProjectilePool projectilePool)
        {
            _scoreService = scoreService;
            _gameState = gameState;
            _ufoPool = ufoPool;
            _asteroidPool = asteroidPool;
            _ufoSpawner = ufoSpawner;
            _asteroidSpawner = asteroidSpawner;
            _playerFactory = playerFactory;
            _gameplayUIModel = gameplayUIModel;
            _projectilePool = projectilePool;
            _assetLoader = assetLoader;
        }
        public void Initialize()
        {
            _scoreService.Initialize();

            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            var (asteroidPrefab, 
                ufoPrefab, 
                playerPrefab,
                projectilePrefab) = await UniTask.WhenAll(
                _assetLoader.LoadAsync<GameObject>(AddressablesConstants.ASTEROIDS_ID),
                _assetLoader.LoadAsync<GameObject>(AddressablesConstants.UFO_ID),
                _assetLoader.LoadAsync<GameObject>(AddressablesConstants.SPACESHIP_ID),
                _assetLoader.LoadAsync<GameObject>(AddressablesConstants.PROJECTILE_ID)
            );
            
            _asteroidPool.Initialize(asteroidPrefab, 10);
            _ufoPool.Initialize(ufoPrefab, 10);
            _projectilePool.Initialize(projectilePrefab, 10);
            _player = _playerFactory.CreatePlayer(playerPrefab);
            
            _asteroidSpawner.Initialize();
            _ufoSpawner.Initialize(_player);
            
            _gameState.Initialize(_player);
            _gameplayUIModel.Initialize(_player);
        }
        
    }
}