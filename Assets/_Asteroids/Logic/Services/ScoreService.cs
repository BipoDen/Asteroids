using System;

namespace Assets._Asteroids.Logic.Services
{
    public class ScoreService : IDisposable
    {
        public int Score { get; private set; }

        private GameState _gameState;
        private ISaveService _saveService;
        
        public event Action<int> OnScoreChanged;

        public ScoreService(GameState gameState, ISaveService saveService)
        {
            _gameState = gameState;
            _saveService = saveService;
        }

        public void Initialize()
        {
            ResetScore();
            _gameState.OnGameRestart += ResetScore;
        }
        
        public void AddScore(int score)
        {
            Score += score;
            if (Score > _saveService.Data.MaxScore)
            {
                _saveService.Data.MaxScore = Score;
                _saveService.Save();
            }
            OnScoreChanged?.Invoke(Score);
        }

        public void ResetScore()
        {
            Score = 0;
            OnScoreChanged?.Invoke(Score);
        }

        public void Dispose()
        {
            _gameState.OnGameRestart -= ResetScore;
        }
    }
}