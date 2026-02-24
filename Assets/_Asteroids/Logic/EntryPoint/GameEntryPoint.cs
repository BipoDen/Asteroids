using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Services;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class GameEntryPoint : IInitializable
    {
        private SpaceshipController _player;
        private ScoreService _scoreService;
        private GameState _gameState;

        public GameEntryPoint( 
            ScoreService scoreService, 
            GameState gameState,
            SpaceshipController player)
        {
            _scoreService = scoreService;
            _gameState = gameState;
            _player = player;
        }
        public void Initialize()
        {
            _scoreService.Initialize();
            _gameState.Initialize(_player);
        }
    }
}