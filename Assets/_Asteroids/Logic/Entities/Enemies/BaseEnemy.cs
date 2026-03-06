using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using Zenject;

namespace Assets._Asteroids.Logic.Entities.Enemies
{
    public class BaseEnemy : SpaceEntity
    {
        protected GameState _gameState;
        protected ScoreService _scoreService;
        
        [Inject]
        public void Construct(SpaceScreen screen, GameState gameState, ScoreService scoreService)
        {
            _spaceScreen = screen;
            _gameState = gameState;
            _scoreService = scoreService;
        }
        
        protected virtual void TryMove()
        {
            
        }

        private void FixedUpdate()
        {
            if(_gameState.IsGamePaused)
                return;
            
            TryTeleport();
            TryMove();
        }

        public virtual void TakeDamage()
        {
            
        }

        protected virtual void AddScore(int value)
        {
            _scoreService.AddScore(value);
        }
    }
}