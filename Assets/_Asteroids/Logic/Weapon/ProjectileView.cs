using System;
using System.Threading;
using Assets._Asteroids.Logic.Entities.Enemies;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets._Asteroids.Logic.Weapon
{
    public class ProjectileView : Bullet
    {
        private float _speed;
        private float _lifeTime;

        public Action OnDied;
        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            Lifetime(_cts.Token).Forget();
        }

        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        public void Init(Transform startPosition, float lifeTime, float speed)
        {
            OnDied = null;
            transform.position = startPosition.position;
            transform.rotation = startPosition.rotation;
            _lifeTime = lifeTime;
            _speed = speed;
        }
        
        private void FixedUpdate()
        {
            transform.position += transform.up * _speed * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<BaseEnemy>() != null)
            {
                other.gameObject.GetComponent<BaseEnemy>().TakeDamage();
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