using Assets._Asteroids.Logic.Addressable;
using Assets._Asteroids.Logic.Constants;
using Assets._Asteroids.Logic.SaveProviders;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.UI;
using Assets._Asteroids.Logic.UI.SaveConflictUI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class MainMenuEntryPoint : IInitializable
    {
        private IInstantiator _instantiator;
        private IAssetLoader _assetLoader;
        private MainMenuUIPresenter _screenPresenter;
        private Canvas _canvas;
        private ISaveService _saveService;
        private PlayerDataProvider _dataProvider;
        private SaveConflictPresenter _saveConflictPresenter;

        public MainMenuEntryPoint(MainMenuUIPresenter screenPresenter, 
            IAssetLoader assetLoader, 
            IInstantiator instantiator, 
            Canvas canvas,
            ISaveService saveService, 
            PlayerDataProvider dataProvider, 
            SaveConflictPresenter saveConflictPresenter)
        {
            _screenPresenter = screenPresenter;
            _assetLoader = assetLoader;
            _instantiator = instantiator;
            _canvas = canvas;
            _saveService = saveService;
            _dataProvider = dataProvider;
            _saveConflictPresenter = saveConflictPresenter;
        }

        public async void Initialize()
        {
            await InitializeUIAsync();
            await InitializeSaveAsync();
        }

        private async UniTask InitializeUIAsync()
        {
            var (mainMenuScreenPrefab, conflictUIPrefab) = await UniTask.WhenAll(
                _assetLoader.LoadAsync<GameObject>(AddressablesConstants.MAIN_MENU_UI_ID), 
                _assetLoader.LoadAsync<GameObject>(AddressablesConstants.MAIN_MENU_CONFLICT_PANEL));
            
            MainMenuUIView uiView = _instantiator.InstantiatePrefabForComponent<MainMenuUIView>(mainMenuScreenPrefab, _canvas.transform);
            _screenPresenter.Initialize(uiView);
            
            SaveConflictView conflictView = _instantiator.InstantiatePrefabForComponent<SaveConflictView>(conflictUIPrefab, _canvas.transform);
            _saveConflictPresenter.Initialize(conflictView);
        }

        private async UniTask InitializeSaveAsync()
        {
            var data = await _saveService.Load();
            _dataProvider.SetSaveData(data);
        }
    }
}