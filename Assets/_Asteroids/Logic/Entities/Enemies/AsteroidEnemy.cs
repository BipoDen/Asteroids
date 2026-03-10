using System;
using Assets._Asteroids.Logic.Services;
using UnityEngine;
using Zenject;

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
        
        private StatsService _statsService;

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
        
        [Inject]
        public void Construct(StatsService statsService)
        {
            _statsService = statsService;
        }

        protected override void TryMove()
        {
            _rigidbody.linearVelocity = _direction * _speed;
        }

        public override void TakeDamage()
        {
            AddScore(_score);
            _statsService.AddAsteroidKill();
            Die();
        }

        private void Die()
        {
            _gameState.OnGameOver -= Die;
            OnDied?.Invoke();
        }
    }
}