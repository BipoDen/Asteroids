using System;
using Assets._Asteroids.Logic.Factory;
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
        
        public event Action<int> OnCountChanged;
        
        [Inject]
        public void Construct(LaserView laserPrefab)
        {
            _laserPrefab = laserPrefab;
        }
        
        public void Init(Transform launchOffset)
        {
            _startPosition = launchOffset;
            _laserView = Object.Instantiate(_laserPrefab, _startPosition);
            _laserView.Init(_startPosition, _laserDistance);
            _laserView.gameObject.SetActive(false);
            LaserCount = MaxLaserCount;
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
            OnCountChanged?.Invoke(LaserCount);
            LaserDuration();
            if (!_isLaserReloading)
            {
                LaserReload();
            }
        }

        private async UniTask LaserDuration()
        {
            _laserView.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_laserDuration));
            _laserView.gameObject.SetActive(false);
            _isLaserActive = false;
        }

        private async UniTask LaserReload()
        {
            _isLaserReloading = true;
            while (LaserCount < MaxLaserCount)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_laserCooldown));
                LaserCount++;
                OnCountChanged?.Invoke(LaserCount);
            }
            _isLaserReloading = false;
        }
    }
}