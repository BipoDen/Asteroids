using System;
using System.Threading;
using Assets._Asteroids.Logic.Analytics;
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
        
        public int MaxLaserCount { get; private set; } = 5;
        public int LaserCount { get; private set; }

        private float _laserDuration = 1.5f;
        private float _laserDistance = 30f;
        private float _laserCooldown = 5f;
        private bool _isLaserActive = false;
        private bool _isLaserReloading = false;

        private CancellationTokenSource _cts;
        private IAnalyticsService _analyticsService;
        
        public event Action<int> OnCountChanged;
        public event Action<float, float> OnReloadTimeChanged;
        public event Action OnShoot;

        [Inject]
        public void Construct(LaserView laserPrefab, IAnalyticsService analyticsService)
        {
            _laserPrefab = laserPrefab;
            _analyticsService = analyticsService;
        }
        
        public void Init(Transform launchOffset)
        {
            _startPosition = launchOffset;
            _laserView = Object.Instantiate(_laserPrefab, _startPosition);
            _laserView.Init(_startPosition, _laserDistance);
            _laserView.gameObject.SetActive(false);
            
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
            OnReloadTimeChanged?.Invoke(0f, _laserCooldown);

            if (_laserView != null)
                _laserView.gameObject.SetActive(false);
        }

        public void HandleFire()
        {
            if(LaserCount <= 0 || _isLaserActive)
                return;
            
            ShootLaser(_startPosition);
        }

        private void ShootLaser(Transform startPosition)
        {
            _isLaserActive = true;
            LaserCount--;
            _analyticsService.OnLaserUsingEvent();
            OnShoot?.Invoke();
            OnCountChanged?.Invoke(LaserCount);
            
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
                await UniTask.Delay(TimeSpan.FromSeconds(_laserDuration), cancellationToken: token);
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
                    float endTime = Time.time + _laserCooldown;

                    while (Time.time < endTime)
                    {
                        token.ThrowIfCancellationRequested();

                        float timeLeft = endTime - Time.time;
                        OnReloadTimeChanged?.Invoke(timeLeft, _laserCooldown);

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