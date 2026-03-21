using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Ads.UnityAds
{
    public class UnityAdsService : IAdService
    {
        private UnityAdsInitializer _initializer;
        private RewardedAds _rewardedAds;
        private InterstitialAds _interstitialAds;

        public UnityAdsService(UnityAdsInitializer initializer, RewardedAds rewardedAds, InterstitialAds interstitialAds)
        {
            _initializer = initializer;
            _rewardedAds = rewardedAds;
            _interstitialAds = interstitialAds;
        }

        public void InitializeAds()
        {
            _initializer.Initialize();
        }

        public void LoadRewardedAd()
        {
            _rewardedAds.LoadAd();
        }

        public async UniTask<bool> ShowRewardedAd()
        {
            return await _rewardedAds.ShowAd();
        }

        public void LoadInterstitialAd()
        {
            _interstitialAds.LoadAd();
        }

        public async UniTask ShowInterstitialAd()
        {
            await _interstitialAds.ShowAd();
        }
    }
}