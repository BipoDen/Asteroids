using System;
using System.Threading;
using Assets._Asteroids.Logic.Analytics;
using Assets._Asteroids.Logic.Audio;
using Assets._Asteroids.Logic.RemoteConfig;
using Assets._Asteroids.Logic.RemoteConfig.Configs.Weapons;
using Assets._Asteroids.Logic.Services;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Assets._Asteroids.Logic.Weapon
{
    public class LaserWeapon : IWeapon
    {
        private LaserView _laserPrefab;
        private LaserView _laserView;
        private Transform _startPosition;
        private LaserWeaponConfig _config;
        
        public int MaxLaserCount { get; private set; }
        public int LaserCount { get; private set; }
        
        private bool _isLaserActive = false;
        private bool _isLaserReloading = false;

        private CancellationTokenSource _cts;
        private IAnalyticsService _analyticsService;
        private IRemoteConfig _configProvider;
        private IAudioService _audioService;
        private IVFXService _vfxService;
        
        public event Action<int> OnCountChanged;
        public event Action<float, float> OnReloadTimeChanged;
        public event Action OnShoot;

        [Inject]
        public void Construct(LaserView laserPrefab, IAnalyticsService analyticsService, IRemoteConfig configProvider, 
            IAudioService audioService, IVFXService vfxService)
        {
            _laserPrefab = laserPrefab;
            _analyticsService = analyticsService;
            _configProvider = configProvider;
            _audioService =  audioService;
            _vfxService = vfxService;
        }
        
        public void Init(Transform launchOffset)
        {
            _config = _configProvider.GetRemoteConfig<LaserWeaponConfig>();
            
            _startPosition = launchOffset;
            _laserView = Object.Instantiate(_laserPrefab, _startPosition);
            _laserView.Init(_startPosition, _config.LaserDistance);
            _laserView.gameObject.SetActive(false);
            MaxLaserCount = _config.MaxLaserCount;
            
            ResetWeapon();
        }

        public void ResetWeapon()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            
            _cts = new CancellationTokenSource();
            
            _isLaserActive = false;
            _isLaserReloading = false;

            LaserCount = MaxLaserCount;
            OnCountChanged?.Invoke(LaserCount);
            OnReloadTimeChanged?.Invoke(0f, _config.LaserCooldown);

            if (_laserView != null)
                _laserView.gameObject.SetActive(false);
        }

        public void HandleFire()
        {
            if(LaserCount <= 0 || _isLaserActive)
                return;
            
            _audioService.PlayLaserShotAudio();
            ShootLaser(_startPosition);
        }

        private void ShootLaser(Transform startPosition)
        {
            _isLaserActive = true;
            LaserCount--;
            _analyticsService.OnLaserUsingEvent();
            OnShoot?.Invoke();
            OnCountChanged?.Invoke(LaserCount);
            _vfxService.CreateLaserFlash(_startPosition);
            LaserDuration(_cts.Token).Forget();
            
            if (!_isLaserReloading)
            {
                LaserReload(_cts.Token).Forget();
            }
        }

        private async UniTask LaserDuration(CancellationToken token)
        {
            try
            {
                _laserView.gameObject.SetActive(true);
                await UniTask.Delay(TimeSpan.FromSeconds(_config.LaserDuration), cancellationToken: token);
            }
            catch (OperationCanceledException) { }
            finally
            {
                if (_laserView != null)
                    _laserView.gameObject.SetActive(false);

                _isLaserActive = false;
            }
        }

        private async UniTask LaserReload(CancellationToken token)
        {
            _isLaserReloading = true;

            try
            {
                while (LaserCount < MaxLaserCount)
                {
                    float endTime = Time.time + _config.LaserCooldown;

                    while (Time.time < endTime)
                    {
                        token.ThrowIfCancellationRequested();

                        float timeLeft = endTime - Time.time;
                        OnReloadTimeChanged?.Invoke(timeLeft, _config.LaserCooldown);

                        await UniTask.Yield(PlayerLoopTiming.Update, token);
                    }

                    LaserCount++;
                    OnCountChanged?.Invoke(LaserCount);
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                _isLaserReloading = false;
            }
        }
    }
}