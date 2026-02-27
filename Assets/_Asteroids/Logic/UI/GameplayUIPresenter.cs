using System;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.Weapon;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.UI
{
    public class GameplayUIPresenter : IDisposable
    {
        private SpaceshipController _player;
        private ScoreService _scoreService;
        private GameState _gameState;
        private IWeapon _secondaryWeapon;

        private GameplayUIView _view;
        
        public GameplayUIPresenter(GameplayUIView view)
        {
            _view = view;
        }
        
        [Inject]
        public void Construct(SpaceshipController player, 
            GameState gameState, 
            ScoreService scoreService, 
            [Inject(Id = GameplayConstants.SECONDARY_WEAPON_TAG)]IWeapon secondaryWeapon)
        {
            _player = player;
            _scoreService = scoreService;
            _gameState = gameState;
            _secondaryWeapon = secondaryWeapon;
            
            _player.OnMove += OnMove;
            _player.OnRotate += OnRotate;
            _scoreService.OnScoreChanged += OnScoreChanged;
            _secondaryWeapon.OnCountChanged += WeaponOnOnCountChanged;
            _secondaryWeapon.OnReloadTimeChanged += WeaponOnOnReloadTimeChanged;
        }

        private void OnRotate(float rotate)
        {
            _view.ChangePlayerRotation(rotate);
        }

        private void OnScoreChanged(int score)
        {
            _view.ChangeScore(score);
        }

        private void WeaponOnOnReloadTimeChanged(float currentCooldown, float cooldown)
        {
            _view.ChangeSecondaryCooldown(currentCooldown, cooldown);
        }

        private void WeaponOnOnCountChanged(int Count)
        {
            _view.ChangeSecondaryCount(Count);
        }

        private void OnMove(Vector2 coords, float speed)
        {
            _view.ChangePlayerCoords(coords, speed);
        }

        public void Dispose()
        {
            _player.OnMove -= OnMove;
            _scoreService.OnScoreChanged -= OnScoreChanged;
            _secondaryWeapon.OnCountChanged -= WeaponOnOnCountChanged;
            _secondaryWeapon.OnReloadTimeChanged -= WeaponOnOnReloadTimeChanged;
        }
    }
}