using System;
using Assets._Asteroids.Logic.Services;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Entities.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class UFOEnemy : BaseEnemy
    {
        private float _speed;
        private Rigidbody2D _rigibody;
        private Transform _target;
        private int _score;
        
        public event Action OnDied;
        
        private StatsService _statsService;

        private void Awake()
        {
            _rigibody = GetComponent<Rigidbody2D>();
        }

        public void Init(Transform player, float speed, int score)
        {
            OnDied = null;
            _speed = speed;
            _target = player;
            _gameState.OnGameOver += Die;
            _score = score;
        }
        
        [Inject]
        public void Construct(StatsService statsService)
        {
            _statsService = statsService;
        }

        protected override void TryMove()
        {
            if (_target == null)
                return;
            
            Vector2 direction = (_target.position - transform.position).normalized;
            _rigibody.linearVelocity = direction * _speed;
        }

        public override void TakeDamage()
        {
            AddScore(_score);
            _statsService.AddUFOKill();
            Die();
        }
        
        private void Die()
        {
            _gameState.OnGameOver -= Die;
            OnDied?.Invoke();
        }
    }
}