using System;
using System.Threading;
using Assets._Asteroids.Logic.Entities.Enemies;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets._Asteroids.Logic.Weapon
{
    public class ProjectileView :MonoBehaviour
    {
        private float _speed;
        private float _lifeTime;

        public event Action OnDied;
        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        public void Init(Transform startPosition, float lifeTime, float speed)
        {
            transform.position = startPosition.position;
            transform.rotation = startPosition.rotation;
            _lifeTime = lifeTime;
            _speed = speed;
            
            Lifetime(_cts.Token).Forget();
        }
        
        private void FixedUpdate()
        {
            transform.position += transform.up * _speed * Time.fixedDeltaTime;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();
                Despawn();  
            }
        }

        private async UniTask Lifetime(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime), cancellationToken: token);
                Despawn();
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        private void Despawn()
        {
            if (_cts == null || _cts.IsCancellationRequested)
                return;
            
            _cts.Cancel();
            OnDied?.Invoke();
        }
    }
}