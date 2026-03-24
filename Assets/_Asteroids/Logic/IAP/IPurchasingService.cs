using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.IAP
{
    public interface IPurchasingService
    {
        UniTask<bool> MakePurchaseAsync (string productId);
    }
}