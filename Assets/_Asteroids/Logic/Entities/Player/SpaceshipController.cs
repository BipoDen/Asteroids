using System;
using Assets._Asteroids.Logic.Audio;
using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Input;
using Assets._Asteroids.Logic.RemoteConfig;
using Assets._Asteroids.Logic.RemoteConfig.Configs;
using Assets._Asteroids.Logic.Services;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Entities.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceshipController : SpaceEntity
    {
        private IInput _input;

        private SpaceshipConfig _config;

        private Rigidbody2D _rigidbody2D;
        private Transform _startPosition;
        private GameState _gameState;
        private IRemoteConfig _configProvider;
        private IAudioService _audioService;
        public event Action OnGameOver;
        public event Action<Vector2, float> OnMove;
        public event Action<float> OnRotate;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        [Inject]
        public void Construct(IInput input, SpaceScreen spaceScreen, [Inject(Id = "StartPosition")] Transform startPosition, GameState gameState, 
            IRemoteConfig configProvider, IAudioService audioService)
        {
            _input = input;
            _spaceScreen = spaceScreen;
            _startPosition = startPosition;
            _gameState = gameState;
            _configProvider = configProvider;
            _audioService = audioService;
            
            _config = _configProvider.GetRemoteConfig<SpaceshipConfig>();
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
            
            Vector2 forwardForce = transform.up * _config.MoveAcceleration * input;
            _rigidbody2D.AddForce(forwardForce, ForceMode2D.Force);
            
            if (_rigidbody2D.linearVelocity.magnitude > _config.MaxSpeed)
            {
                _rigidbody2D.linearVelocity =
                    _rigidbody2D.linearVelocity.normalized * _config.MaxSpeed;
            }
            
            OnMove?.Invoke(transform.position, _rigidbody2D.linearVelocity.magnitude);
        }

        private void TryRotate(float input)
        {
            if(_gameState.IsGamePaused)
                return;
            
            OnRotate?.Invoke(transform.rotation.eulerAngles.z);
            
            if (!Mathf.Approximately(input, 0f))
            { 
                float rotationAngle = -input * _config.RotateSpeed * Time.fixedDeltaTime;
                _rigidbody2D.MoveRotation(_rigidbody2D.rotation + rotationAngle);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<BaseEnemy>() != null)
            {
                Die();
            }
        }

        private void Die()
        {
            OnGameOver?.Invoke();
            _audioService.PlayExplosionAudio(1);
        }
        
        public void ResetPosition()
        {
            transform.position = _startPosition.position;
            transform.rotation = _startPosition.rotation;
            OnMove?.Invoke(Vector2.zero, 0);
            OnRotate?.Invoke(0);
        }
    }
}