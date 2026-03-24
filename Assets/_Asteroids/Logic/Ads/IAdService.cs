using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Ads
{
    public interface IAdService
    {
        UniTask<bool> ShowRewardedAd();
        UniTask ShowInterstitialAd();
    }
}