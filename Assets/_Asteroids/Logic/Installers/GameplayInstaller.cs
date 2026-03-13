using Assets._Asteroids.Logic.Addressable;
using Assets._Asteroids.Logic.Constants;
using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.EntryPoint;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Input;
using Assets._Asteroids.Logic.Repository;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.UI;
using Assets._Asteroids.Logic.Weapon;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _playerStartPosition;
        [SerializeField] private ProjectileView _projectilePrefab;
        [SerializeField] private LaserView _laserPrefab;
        [SerializeField] private GameplayUIView _gameplayView;
        [SerializeField] private GameOverView _gameOverView;
        public override void InstallBindings()
        {
            BindInput();
            BindServices();
            BindPlayer();
            BindEnemies();
            BindUI();
            Container.BindInterfacesTo<GameplayEntryPoint>().AsSingle();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();
            Container.Bind<SpaceScreen>().FromNew().AsSingle().WithArguments(Camera.main);
        }

        private void BindServices()
        {
            Container.Bind<StatsService>().FromNew().AsSingle();
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
            
            Container.Bind<SpaceshipFactory>().FromNew().AsSingle();
        }

        private void BindEnemies()
        {
            Container.Bind<EnemyRepository>().FromNew().AsSingle();
            
            Container.Bind<EnemyPool<AsteroidEnemy>>()
                .AsSingle() 
                .WithArguments("Asteroids");
            
            Container.Bind<AsteroidFactory>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<AsteroidSpawner>().FromNew().AsSingle();

            Container.Bind<EnemyPool<UFOEnemy>>()
                .AsSingle() 
                .WithArguments("UFOs");
            
            Container.Bind<UFOFactory>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<UFOSpawner>().FromNew().AsSingle();
        }

        private void BindUI()
        {
            GameplayUIView uiView = Container.InstantiatePrefabForComponent<GameplayUIView>(_gameplayView, _canvas.transform);
            Container.Bind<GameplayUIView>().FromInstance(uiView);
            Container.Bind<GameplayUIModel>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayUIPresenter>().AsSingle();
            
            GameOverView uiGameOverView = Container.InstantiatePrefabForComponent<GameOverView>(_gameOverView, _canvas.transform);
            Container.Bind<GameOverView>().FromInstance(uiGameOverView);
            Container.BindInterfacesAndSelfTo<GameOverPresenter>().AsSingle();
        }
    }
}