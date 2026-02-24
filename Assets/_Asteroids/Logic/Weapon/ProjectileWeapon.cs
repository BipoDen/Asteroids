using System;
using Assets._Asteroids.Logic.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;


namespace Assets._Asteroids.Logic.Weapon
{
    public class ProjectileWeapon : IWeapon
    {
        private bool _isReloading = false;
        private float _delay = 0.5f;
        private float _bulletLifeTime = 5f;
        private float _bulletSpeed = 7.5f;
        private Transform _startPosition;
        
        private ProjectileFactory _factory;
        public event Action<int> OnCountChanged;
        public event Action<float, float> OnReloadTimeChanged;

        [Inject]
        public void Construct(ProjectileFactory factory)
        {
            _factory = factory;
        }

        public void Init(Transform launchOffset)
        {
            _startPosition = launchOffset;
        }
        
        public void HandleFire()
        {
            if(_isReloading)
                return;
            
            _isReloading = true;
            
            CreateBullet(_startPosition);    
            
            Reload(_delay).Forget();
        }
        
        private async UniTask Reload(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay),  DelayType.UnscaledDeltaTime);
            _isReloading = false;
        }

        private void CreateBullet(Transform startPosition)
        {
            _factory.Create(startPosition, _bulletLifeTime, _bulletSpeed);
        }
    }
}