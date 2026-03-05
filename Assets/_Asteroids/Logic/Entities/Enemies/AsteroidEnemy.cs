using System;
using UnityEngine;

namespace Assets._Asteroids.Logic.Entities.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AsteroidEnemy : BaseEnemy
    {
        private Vector2 _direction;
        private int _score;
        private float _speed;
        
        public event Action OnDied;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Initialize(float speed, float size, Vector2 direction, int score)
        {
            OnDied = null;
            _gameState.OnGameOver += Die;
            transform.localScale = new Vector3(size, size, size);
            _speed = speed;
            _direction = direction;
            _score = score;
        }

        protected override void TryMove()
        {
            _rigidbody.linearVelocity = _direction * _speed;
        }

        public override void TakeDamage()
        {
            AddScore(_score);
            Die();
        }

        private void Die()
        {
            _gameState.OnGameOver -= Die;
            OnDied?.Invoke();
        }
    }
}