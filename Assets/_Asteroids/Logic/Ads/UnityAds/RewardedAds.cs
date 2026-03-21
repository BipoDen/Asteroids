using Cysharp.Threading.Tasks;
using UnityEngine.Advertisements;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Ads.UnityAds
{
    public class RewardedAds : IInitializable, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private string _androidAdUnitId = "Rewarded_Android";
        private string _iOSAdUnitId = "Rewarded_iOS";
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

        public async UniTask<bool> ShowAd()
        {
            _completionSource = new UniTaskCompletionSource<bool>();
            Debug.Log("Showing Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
            return await _completionSource.Task;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"Unity Ads successfully loaded: {placementId}");
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"Unity Ads failed to load {placementId}: {error.ToString()} - {message}");
        }
        
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId == _adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("[Advertisement]: Ad Completed");
                _completionSource.TrySetResult(true);
            }
            else
            {
                Debug.Log("[Advertisement]: Ad Failed");
                _completionSource.TrySetResult(false);
            }
        }
        
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogWarning($"[Ads] Show failure: {error} — {message}");
            _completionSource?.TrySetResult(false);
        }
        
        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"Unity Ads started showing: {placementId}");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"Unity Ads was clicked: {placementId}");
        }
    }
}