using System;

namespace Assets._Asteroids.Logic.UI
{
    public class MainMenuUIPresenter : IDisposable
    {
        private MainMenuUIView _view;
        
        public MainMenuUIPresenter(MainMenuUIView view)
        {
            _view = view;
            
            _view.OnRemoveAdsClick.AddListener(TryPurchase);
            _view.OnStartPlay.AddListener(StartGame);
        }

        private void TryPurchase()
        {
            
        }

        private void StartGame()
        {
            
        }

        public void Dispose()
        {
            _view.OnRemoveAdsClick.RemoveListener(TryPurchase);
            _view.OnStartPlay.RemoveListener(StartGame);
        }
    }
}