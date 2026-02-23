using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Services;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class GameEntryPoint : IInitializable
    {
        private AsteroidSpawner _asteroidSpawner;
        private SpaceshipController _player;
        private UFOSpawner _ufoSpawner;
        private ScoreService _scoreService;
        private GameState _gameState;

        public GameEntryPoint(AsteroidSpawner asteroidSpawner,  
            UFOSpawner ufoSpawner, 
            ScoreService scoreService, 
            GameState gameState,
            SpaceshipController player)
        {
            _asteroidSpawner = asteroidSpawner;
            _ufoSpawner = ufoSpawner;
            _scoreService = scoreService;
            _gameState = gameState;
            _player = player;
        }
        public void Initialize()
        {
            _asteroidSpawner.Initialize();
            _ufoSpawner.Initialize();
            _scoreService.Initialize();
            _gameState.Initialize(_player);
        }
    }
}