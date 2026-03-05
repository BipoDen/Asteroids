using System;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.Weapon;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.UI
{
    public class GameplayUIModel : IDisposable
    {
        private readonly SpaceshipController _player;
        private readonly ScoreService _scoreService;
        private readonly IWeapon _secondaryWeapon;

        public event Action<Vector2, float> OnPlayerMoved;
        public event Action<float> OnPlayerRotated;
        public event Action<int> OnScoreChanged;
        public event Action<int> OnSecondaryCountChanged;
        public event Action<float, float> OnSecondaryCooldownChanged;

        public GameplayUIModel(SpaceshipController player,
            ScoreService scoreService,
            [Inject(Id = GameplayConstants.SECONDARY_WEAPON_TAG)] IWeapon secondaryWeapon)
        {
            _player = player;
            _scoreService = scoreService;
            _secondaryWeapon = secondaryWeapon;

            _player.OnMove += HandleMove;
            _player.OnRotate += HandleRotate;
            _scoreService.OnScoreChanged += HandleScoreChanged;
            _secondaryWeapon.OnCountChanged += HandleSecondaryCountChanged;
            _secondaryWeapon.OnReloadTimeChanged += HandleCooldownChanged;
        }

        private void HandleMove(Vector2 coords, float speed)
            => OnPlayerMoved?.Invoke(coords, speed);

        private void HandleRotate(float angle)
            => OnPlayerRotated?.Invoke(angle);
        
        private void HandleScoreChanged(int count)
            => OnScoreChanged?.Invoke(count);

        private void HandleSecondaryCountChanged(int count)
            => OnSecondaryCountChanged?.Invoke(count);
        
        private void HandleCooldownChanged(float current, float max)
            => OnSecondaryCooldownChanged?.Invoke(current, max);


        public void Dispose()
        {
            _player.OnMove -= HandleMove;
            _player.OnRotate -= HandleRotate;
            _scoreService.OnScoreChanged -= HandleScoreChanged;
            _secondaryWeapon.OnCountChanged -= HandleSecondaryCountChanged;
            _secondaryWeapon.OnReloadTimeChanged -= HandleCooldownChanged;
        }
    }
}