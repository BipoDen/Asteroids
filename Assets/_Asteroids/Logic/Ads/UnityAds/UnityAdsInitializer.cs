using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace Assets._Asteroids.Logic.Ads.UnityAds
{
    public class UnityAdsInitializer : IUnityAdsInitializationListener, IInitializable
    {
        private string _androidGameId = "6067860";
        private string _iosGameId = "6067861";
        private bool _testMode = true;
        private string _gameId;
        
        public async void Initialize()
        {
            await StartWork();
        }
        
        private async UniTask StartWork()
        { 
#if UNITY_IOS
            _gameId = _iosGameId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId;
#endif
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }
        
        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}