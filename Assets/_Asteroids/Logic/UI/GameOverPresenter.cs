using System;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;

namespace Assets._Asteroids.Logic.UI
{
    public class GameOverPresenter : IDisposable
    {
        private GameOverView _view;
        
        private ScoreService _scoreService;
        private GameState _gameState;
        private SaveData _saveData;
        
        public GameOverPresenter(ScoreService scoreService, GameState gameState, SaveData saveData)
        {
            _scoreService = scoreService;
            _gameState = gameState;
            _saveData = saveData;
        }

        public void Initialize(GameOverView view)
        {
            _view = view;
            _view.OnRestart.AddListener(Restart);
            
            _gameState.OnGameOver += Show;
            Hide();
        }

        private void Show()
        {
            _view.gameObject.SetActive(true);
            _view.SetMaxScore(_saveData.MaxScore, _scoreService.Score);
            _view.ShowScore(_scoreService.Score);
        }

        private void Hide()
        {
            _view.gameObject.SetActive(false);
        }

        private void Restart()
        {
            Hide();
            _gameState.RestartGame();
        }

        public void Dispose()
        {
            _gameState.OnGameOver -= Show;
            _view.OnRestart.RemoveListener(Restart);
        }
    }
}