using System.Collections.Generic;
using Assets._Asteroids.Logic.Constants;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Input;
using Assets._Asteroids.Logic.Services;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        private IInput _input;
        private GameState _gameState;
        private StatsService _statsService;
        private List<IWeapon> _weapons;
        
        [SerializeField] private Transform _launchOffset;

        [Inject]
        public void Construct(IInput input, 
            GameState gameState,
            StatsService statsService, 
            [Inject(Id = GameplayConstants.PRIMARY_WEAPON_TAG)] IWeapon primary,
            [Inject(Id = GameplayConstants.SECONDARY_WEAPON_TAG)] IWeapon secondary)
        {
            _input = input;
            _gameState = gameState;
            _statsService = statsService;
            _weapons = new List<IWeapon> { primary,  secondary };
            foreach (var weapon in _weapons)
            {
                weapon.Init(_launchOffset);
            }
            
            _gameState.OnGameRestart += RestartWeapons;
        }

        private void RestartWeapons()
        {
            _weapons[0].ResetWeapon();
            _weapons[0].OnShoot += AddPrimaryShot;
            _weapons[1].ResetWeapon();
            _weapons[1].OnShoot += AddSecondaryShot;
        }

        private void Update()
        {
            if(_gameState.IsGamePaused)
                return;
            
            if (_input.isShootingPrimary())
            {
                _weapons[0].HandleFire();
            }

            if (_input.isShootingSecondary())
            {
                _weapons[1].HandleFire();
            }
        }

        private void OnDestroy()
        {
            _gameState.OnGameRestart -= RestartWeapons;
            _weapons[0].OnShoot -= AddPrimaryShot;
            _weapons[1].OnShoot -= AddSecondaryShot;
        }

        private void AddPrimaryShot()
        {
            _statsService.AddPrimaryShot();
        }

        private void AddSecondaryShot()
        {
            _statsService.AddSecondaryShot();
        }
    }
}