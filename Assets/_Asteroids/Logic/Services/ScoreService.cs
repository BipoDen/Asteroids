using System;

namespace Assets._Asteroids.Logic.Services
{
    public class ScoreService : IDisposable
    {
        public int Score { get; private set; }
        public event Action<int> OnScoreChanged;
        private GameState _gameState;

        public ScoreService(GameState gameState)
        {
            _gameState = gameState;
        }

        public void Initialize()
        {
            ResetScore();
            _gameState.OnGameRestart += ResetScore;
        }
        
        public void AddScore(int score)
        {
            Score += score;
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