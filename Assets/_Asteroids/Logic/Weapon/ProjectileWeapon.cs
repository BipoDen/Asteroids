using System;
using Assets._Asteroids.Logic.Audio;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.RemoteConfig;
using Assets._Asteroids.Logic.RemoteConfig.Configs.Weapons;
using Assets._Asteroids.Logic.Services;
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
        private IRemoteConfig _configProvider;
        private IAudioService _audioService;
        private IVFXService _vfxService;
        
        public event Action<int> OnCountChanged;
        public event Action<float, float> OnReloadTimeChanged;
        public event Action OnShoot;

        [Inject]
        public void Construct(ProjectileFactory factory, IRemoteConfig configProvider, IAudioService audioService, IVFXService vfxService)
        {
            _factory = factory;
            _configProvider = configProvider;
            _audioService = audioService;
            _vfxService = vfxService;
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
            _audioService.PlayProjectileShotAudio();
            _vfxService.CreateProjectileFlash(_startPosition);
            CreateBullet(_startPosition);    
            OnShoot?.Invoke();
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