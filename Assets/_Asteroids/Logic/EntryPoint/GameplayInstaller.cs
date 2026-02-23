using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Input;
using Assets._Asteroids.Logic.Repository;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.Weapon;
using Zenject;
using UnityEngine;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private Vector2 _upLeftBorder;
        [SerializeField] private Vector2 _downRightBorder;
        [SerializeField] private SpaceshipController _playerShip;
        [SerializeField] private Transform _playerStartPosition;
        [SerializeField] private ProjectileView _projectilePrefab;
        [SerializeField] private LaserView _laserPrefab;
        [SerializeField] private AsteroidEnemy _asteroidPrefab;
        [SerializeField] private UFOEnemy _ufoPrefab;
        public override void InstallBindings()
        {
            BindInput();
            BindServices();
            BindPlayer();
            BindEnemies();
            Container.BindInterfacesAndSelfTo<GameEntryPoint>().AsSingle();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();
            Container.Bind<SpaceScreen>().FromNew().AsSingle().WithArguments(_upLeftBorder, _downRightBorder);
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
            
            Container.Bind<IWeapon>().WithId("Primary").To<ProjectileWeapon>().AsSingle();
            Container.Bind<IWeapon>().WithId("Secondary").To<LaserWeapon>().AsSingle();
            
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
    }
}