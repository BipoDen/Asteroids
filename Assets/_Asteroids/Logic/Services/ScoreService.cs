using System;
using Assets._Asteroids.Logic.Gameplay;

namespace Assets._Asteroids.Logic.Services
{
    public class ScoreService : IDisposable
    {
        public int Score { get; private set; }

        private GameState _gameState;
        private ISaveService _saveService;
        private SaveData _saveData;
        
        public event Action<int> OnScoreChanged;

        public ScoreService(GameState gameState, ISaveService saveService, SaveData saveData)
        {
            _gameState = gameState;
            _saveService = saveService;
            _saveData = saveData;
        }

        public void Initialize()
        {
            ResetScore();
            _gameState.OnGameRestart += ResetScore;
        }
        
        public void AddScore(int score)
        {
            Score += score;
            if (Score > _saveData.MaxScore)
            {
                _saveData.MaxScore = Score;
                _saveService.Save(_saveData);
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