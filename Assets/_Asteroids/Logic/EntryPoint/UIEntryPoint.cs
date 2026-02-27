using System;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.Weapon;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class UIEntryPoint : IInitializable
    {
        private ScoreService _scoreService;
        private GameState _gameState;
        private SpaceshipController _player;
        private IWeapon _secondaryWeapon;
        
        

        public UIEntryPoint(ScoreService scoreService,
            GameState gameState,
            SpaceshipController player, 
            [Inject(Id = GameplayConstants.SECONDARY_WEAPON_TAG)] IWeapon weapon)
        {
            
        }
        
        public void Initialize()
        {
            
        }
    }
}