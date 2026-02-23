using System;
using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Input;
using Assets._Asteroids.Logic.Services;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Entities.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceshipController : SpaceEntity
    {
        private IInput _input;
        
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _moveAcceleration;
        [SerializeField] private float _maxSpeed;

        private Rigidbody2D _rigidbody2D;
        private Transform _startPosition;
        private GameState _gameState;
        public event Action OnGameOver;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        [Inject]
        public void Construct(IInput input, SpaceScreen spaceScreen, [Inject(Id = "StartPosition")] Transform startPosition, GameState gameState)
        {
            _input = input;
            _spaceScreen = spaceScreen;
            _startPosition = startPosition;
            _gameState = gameState;
        }
        
        private void FixedUpdate()
        {
            TryMove(_input.Move());
            TryRotate(_input.Rotate());
            TryTeleport();
        }
        
        private void TryMove(float input)
        {
            if (input <= 0f || _gameState.IsGamePaused)
                return;
            
            Vector2 forwardForce = transform.up * _moveAcceleration * input;
            _rigidbody2D.AddForce(forwardForce, ForceMode2D.Force);
                
            if (_rigidbody2D.linearVelocity.magnitude > _maxSpeed)
            {
                _rigidbody2D.linearVelocity =
                    _rigidbody2D.linearVelocity.normalized * _maxSpeed;
            }
        }

        private void TryRotate(float input)
        {
            if(_gameState.IsGamePaused)
                return;
            
            if (!Mathf.Approximately(input, 0f))
            { 
                float rotationAngle = -input * _rotateSpeed * Time.fixedDeltaTime;
                _rigidbody2D.MoveRotation(_rigidbody2D.rotation + rotationAngle);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<BaseEnemy>() != null)
            {
                OnGameOver?.Invoke();
            }
        }
        
        public void ResetPosition()
        {
            transform.position = _startPosition.position;
            transform.rotation = _startPosition.rotation;
        }
    }
}