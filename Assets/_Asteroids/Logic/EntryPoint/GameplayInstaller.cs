using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Input;
using Assets._Asteroids.Logic.Repository;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.UI;
using Assets._Asteroids.Logic.Weapon;
using Zenject;
using UnityEngine;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _canvas;
        
        [SerializeField] private Vector2 _upLeftBorder;
        [SerializeField] private Vector2 _downRightBorder;
        [SerializeField] private SpaceshipController _playerShip;
        [SerializeField] private Transform _playerStartPosition;
        [SerializeField] private ProjectileView _projectilePrefab;
        [SerializeField] private LaserView _laserPrefab;
        [SerializeField] private AsteroidEnemy _asteroidPrefab;
        [SerializeField] private UFOEnemy _ufoPrefab;
        [SerializeField] private GameplayUIView _gameplayView;
        [SerializeField] private GameOverView _gameOverView;
        public override void InstallBindings()
        {
            BindInput();
            BindServices();
            BindPlayer();
            BindEnemies();
            BindUI();
            Container.BindInterfacesAndSelfTo<GameEntryPoint>().AsSingle();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();
            Container.Bind<SpaceScreen>().FromNew().AsSingle().WithArguments(Camera.main);
        }

        private void BindServices()
        {
            Container.Bind<GameState>().FromNew().AsSingle();
            Container.Bind<ScoreService>().FromNew().AsSingle();
        }
        
        private void BindPlayer()
        {
            Container.Bind<Transform>().WithId("StartPosition").FromInstance(_playerStartPosition).AsSingle();
            Container.Bind<LaserView>().FromInstance(_laserPrefab).AsSingle();

            Container.BindMemoryPool<ProjectileView, ProjectilePool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_projectilePrefab)
                .UnderTransformGroup("Projectiles");
            
            Container.Bind<ProjectileFactory>().FromNew().AsSingle();
            
            Container.Bind<IWeapon>().WithId(GameplayConstants.PRIMARY_WEAPON_TAG).To<ProjectileWeapon>().AsSingle();
            Container.Bind<IWeapon>().WithId(GameplayConstants.SECONDARY_WEAPON_TAG).To<LaserWeapon>().AsSingle();
            
            SpaceshipController player = Container.InstantiatePrefabForComponent<SpaceshipController>(_playerShip, Vector3.zero, Quaternion.identity, null);
            Container.BindInterfacesAndSelfTo<SpaceshipController>().FromInstance(player).AsSingle();
        }

        private void BindEnemies()
        {
            Container.Bind<EnemyRepository>().FromNew().AsSingle();
            
            Container.BindMemoryPool<AsteroidEnemy, AsteroidsPool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_asteroidPrefab)
                .UnderTransformGroup("Asteroids");
            
            Container.Bind<AsteroidFactory>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<AsteroidSpawner>().FromNew().AsSingle();

            Container.BindMemoryPool<UFOEnemy, UFOPool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_ufoPrefab)
                .UnderTransformGroup("UFOs");
            Container.Bind<UFOFactory>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<UFOSpawner>().FromNew().AsSingle();
        }

        private void BindUI()
        {
            GameplayUIView uiView = Container.InstantiatePrefabForComponent<GameplayUIView>(_gameplayView, _canvas.transform);
            GameplayUIPresenter gameplayPresenter = new GameplayUIPresenter(uiView);
            Container.QueueForInject(gameplayPresenter);
            Container.BindInterfacesAndSelfTo<GameplayUIPresenter>().FromInstance(gameplayPresenter).AsSingle();
            
            GameOverView uiGameOverView = Container.InstantiatePrefabForComponent<GameOverView>(_gameOverView, _canvas.transform);
            GameOverPresenter gameOverPresenter = new GameOverPresenter(uiGameOverView);
            Container.QueueForInject(gameOverPresenter);
            Container.BindInterfacesAndSelfTo<GameOverPresenter>().FromInstance(gameOverPresenter).AsSingle();
        }
    }
}