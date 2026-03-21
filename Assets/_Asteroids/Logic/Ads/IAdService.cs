using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Ads
{
    public interface IAdService
    {
        void InitializeAds();
        void LoadRewardedAd();
        UniTask<bool> ShowRewardedAd();
        void LoadInterstitialAd();
        UniTask ShowInterstitialAd();
    }
}