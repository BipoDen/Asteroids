using System;
using Assets._Asteroids.Logic.Ads;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.UI
{
    public class GameOverPresenter : IDisposable
    {
        private GameOverView _view;
        
        private ScoreService _scoreService;
        private GameState _gameState;
        private SaveData _saveData;
        private IAdService _adService;
        
        public GameOverPresenter(ScoreService scoreService, GameState gameState, SaveData saveData, IAdService adService)
        {
            _scoreService = scoreService;
            _gameState = gameState;
            _saveData = saveData;
            _adService = adService;
        }

        public void Initialize(GameOverView view)
        {
            _view = view;
            _view.OnRestart.AddListener(Restart);
            _view.OnAdClick.AddListener(ShowAd);
            
            _gameState.OnGameOver += Show;
            Hide();
        }

        private void ShowAd()
        {
            ContinueGame().Forget();
            _view.SetAdButtonInteractable(false);
        }

        private async UniTask ContinueGame()
        {
            if (!_saveData.IsAdDisabled)
            {
                bool result = await _adService.ShowRewardedAd();
                if (result)
                {
                    _gameState.ContinueGame();
                    Hide();
                }
            }
            else
            {
                _gameState.ContinueGame();
                Hide();
            }
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
            _view.SetAdButtonInteractable(true);
        }

        public void Dispose()
        {
            _gameState.OnGameOver -= Show;
            _view.OnRestart.RemoveListener(Restart);
            _view.OnAdClick.RemoveListener(ShowAd);
        }
    }
}