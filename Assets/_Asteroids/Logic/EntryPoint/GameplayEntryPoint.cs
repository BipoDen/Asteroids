using Assets._Asteroids.Logic.Addressable;
using Assets._Asteroids.Logic.Constants;
using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.UI;
using Assets._Asteroids.Logic.Weapon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class GameplayEntryPoint : IInitializable
    {
        private SpaceshipController _player;
        private readonly ScoreService _scoreService;
        private readonly GameState _gameState;
        private readonly IAssetLoader _assetLoader;
        private readonly EnemyPool<AsteroidEnemy> _asteroidPool;
        private readonly EnemyPool<UFOEnemy> _ufoPool;
        private readonly AsteroidSpawner _asteroidSpawner;
        private readonly UFOSpawner _ufoSpawner;
        private readonly SpaceshipFactory _playerFactory;
        private readonly GameplayUIModel _gameplayUIModel;
        private readonly ProjectilePool _projectilePool;
        private readonly IInstantiator _instantiator;
        private readonly Canvas _canvas;
        private readonly GameplayUIPresenter _gameplayUIPresenter;
        private readonly GameOverPresenter _gameoverUIPresenter;
        
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
            ProjectilePool projectilePool, 
            IInstantiator instantiator, 
            Canvas canvas, 
            GameplayUIPresenter gameplayUIPresenter, 
            GameOverPresenter gameoverUIPresenter)
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
            _canvas = canvas;
            _gameplayUIPresenter = gameplayUIPresenter;
            _gameoverUIPresenter = gameoverUIPresenter;
            _assetLoader = assetLoader;
            _instantiator =  instantiator;
        }
        public void Initialize()
        {
            _scoreService.Initialize();

            InitializeEntitiesAsync().Forget();
            InitializeUI().Forget();
        }

        private async UniTask InitializeEntitiesAsync()
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
            
            _asteroidPool.Initialize(asteroidPrefab.GetComponent<AsteroidEnemy>(), 10);
            _ufoPool.Initialize(ufoPrefab.GetComponent<UFOEnemy>(), 10);
            _projectilePool.Initialize(projectilePrefab, 10);
            _player = _playerFactory.CreatePlayer(playerPrefab.GetComponent<SpaceshipController>());
            
            _asteroidSpawner.Initialize();
            _ufoSpawner.Initialize(_player);
            _gameState.Initialize(_player);
            _gameplayUIModel.Initialize(_player);
        }
        
        private async UniTask InitializeUI()
        {
            var (gameplayUIPrefab, gameoverPrefab) = await UniTask.WhenAll(
                _assetLoader.LoadAsync<GameObject>(AddressablesConstants.GAMEPLAY_UI_ID),
                _assetLoader.LoadAsync<GameObject>(AddressablesConstants.GAME_OVER_UI_ID)
            );
            
            GameplayUIView uiView = _instantiator.InstantiatePrefabForComponent<GameplayUIView>(gameplayUIPrefab, _canvas.transform);
            _gameplayUIPresenter.Initialize(uiView, _gameplayUIModel);
            
            GameOverView uiGameOverView = _instantiator.InstantiatePrefabForComponent<GameOverView>(gameoverPrefab, _canvas.transform);
            _gameoverUIPresenter.Initialize(uiGameOverView);
        }
    }
}