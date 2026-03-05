using System;
using Assets._Asteroids.Logic.Services;
using Zenject;

namespace Assets._Asteroids.Logic.UI
{
    public class GameOverPresenter : IDisposable
    {
        private GameOverView _view;
        
        private ScoreService _scoreService;
        private GameState _gameState;
        private ISaveService _saveService;
        
        public GameOverPresenter(GameOverView view)
        {
            _view = view;
        }
        
        [Inject]
        public void Construct(ScoreService scoreService, GameState gameState, ISaveService saveService)
        {
            _scoreService = scoreService;
            _gameState = gameState;
            _saveService = saveService;
            
            _gameState.OnGameOver += Show;
            _view.OnRestart.AddListener(Restart);
            
            Hide();
        }

        private void Show()
        {
            _view.gameObject.SetActive(true);
            _view.SetMaxScore(_saveService.Data.MaxScore, _scoreService.Score);
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