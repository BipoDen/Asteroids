using System;
using Assets._Asteroids.Logic.Constants;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.IAP;
using Assets._Asteroids.Logic.Services;

namespace Assets._Asteroids.Logic.UI
{
    public class MainMenuUIPresenter : IDisposable
    {
        private MainMenuUIView _view;

        private IPurchasingService _purchaseService;
        private SceneLoader _sceneLoader;
        private SaveData _saveData;

        public MainMenuUIPresenter(IPurchasingService purchaseService, SceneLoader sceneLoader, SaveData saveData)
        {
            _purchaseService = purchaseService;
            _sceneLoader = sceneLoader;
            _saveData = saveData;
        }

        public void Initialize(MainMenuUIView view)
        {
            _view = view;
            
            if(_saveData.IsAdDisabled)
                _view.SetRemovingAdsInteractable(false);
            
            _view.OnRemoveAdsClick.AddListener(TryPurchase);
            _view.OnStartPlay.AddListener(StartGame);
        }

        private async void TryPurchase()
        {
            var isPurchased = await _purchaseService.MakePurchaseAsync(ProductsConstants.NO_ADS_PRODUCT);
            if (isPurchased)
                _view.SetRemovingAdsInteractable(false);
        }

        private void StartGame()
        {
            _sceneLoader.LoadScene(GameplayConstants.GAMEPLAY_SCENE_NAME);
        }

        public void Dispose()
        {
            _view.OnRemoveAdsClick.RemoveListener(TryPurchase);
            _view.OnStartPlay.RemoveListener(StartGame);
        }
    }
}