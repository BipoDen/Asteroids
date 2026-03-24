using System;
using Assets._Asteroids.Logic.Addressable;
using Assets._Asteroids.Logic.Constants;
using Assets._Asteroids.Logic.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.EntryPoint
{
    public class MainMenuEntryPoint : IInitializable
    {
        private IInstantiator _instantiator;
        private IAssetLoader _assetLoader;
        private MainMenuUIPresenter _presenter;
        private Canvas _canvas;

        public MainMenuEntryPoint(MainMenuUIPresenter presenter, IAssetLoader assetLoader, IInstantiator instantiator, Canvas canvas)
        {
            _presenter = presenter;
            _assetLoader = assetLoader;
            _instantiator = instantiator;
            _canvas = canvas;
        }

        public void Initialize()
        {
            InitializeUIAsync();
        }

        private async UniTask InitializeUIAsync()
        {
            var uiPrefab = await _assetLoader.LoadAsync<GameObject>(AddressablesConstants.MAIN_MENU_UI_ID);
            MainMenuUIView uiView = _instantiator.InstantiatePrefabForComponent<MainMenuUIView>(uiPrefab, _canvas.transform);
            _presenter.Initialize(uiView);
        }
    }
}