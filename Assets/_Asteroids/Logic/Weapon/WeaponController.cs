using System;
using System.Collections.Generic;
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
        private List<IWeapon> _weapons;
        
        [SerializeField] private Transform _launchOffset;

        [Inject]
        public void Construct(IInput input, 
            GameState gameState, 
            [Inject(Id = "Primary")] IWeapon primary,
            [Inject(Id = "Secondary")] IWeapon secondary)
        {
            _input = input;
            _gameState = gameState;
            _weapons = new List<IWeapon> { primary,  secondary };
            foreach (var weapon in _weapons)
            {
                weapon.Init(_launchOffset);
            }
        }
        
        private void FixedUpdate()
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
    }
}