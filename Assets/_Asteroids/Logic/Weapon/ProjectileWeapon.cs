using System;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.RemoteConfig;
using Assets._Asteroids.Logic.RemoteConfig.Configs.Weapons;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;


namespace Assets._Asteroids.Logic.Weapon
{
    public class ProjectileWeapon : IWeapon
    {
        private bool _isReloading;
        private Transform _startPosition;
        private BulletWeaponConfig _config;
        
        private ProjectileFactory _factory;
        public event Action<int> OnCountChanged;
        public event Action<float, float> OnReloadTimeChanged;
        public event Action OnShoot;
        
        private IRemoteConfig _configProvider;

        [Inject]
        public void Construct(ProjectileFactory factory, IRemoteConfig configProvider)
        {
            _factory = factory;
            _configProvider = configProvider;
        }

        public void Init(Transform launchOffset)
        {
            _config = _configProvider.GetRemoteConfig<BulletWeaponConfig>();
            _startPosition = launchOffset;
        }

        public void ResetWeapon()
        {
            
        }

        public void HandleFire()
        {
            if(_isReloading)
                return;
            
            _isReloading = true;
            
            CreateBullet(_startPosition);    
            
            Reload(_config.Delay).Forget();
        }
        
        private async UniTask Reload(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay),  DelayType.UnscaledDeltaTime);
            _isReloading = false;
        }

        private void CreateBullet(Transform startPosition)
        {
            _factory.Create(startPosition, _config.BulletLifeTime, _config.BulletSpeed);
        }
    }
}