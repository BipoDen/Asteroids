using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace Assets._Asteroids.Logic.Ads.UnityAds
{
    public class InterstitialAds : IInitializable, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private string _androidAdUnitId = "Interstitial_Android";
        private string _iOSAdUnitId = "Interstitial_iOS";
        private string _adUnitId;
        
        private UniTaskCompletionSource<bool> _completionSource;
        
        public void Initialize()
        {
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSAdUnitId
                : _androidAdUnitId;
        }

        public void LoadAd()
        {
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        public async UniTask ShowAd()
        {
            Debug.Log("Showing Ad: " + _adUnitId);
            _completionSource = new UniTaskCompletionSource<bool>();
            Advertisement.Show(_adUnitId, this);
            await _completionSource.Task;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("Interstitial Ad Loaded");
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId) { }

        public void OnUnityAdsShowClick(string placementId) { }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            _completionSource.TrySetResult(true);
        }
    }
}