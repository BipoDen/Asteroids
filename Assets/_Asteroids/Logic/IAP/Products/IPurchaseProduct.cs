using UnityEngine.Purchasing;

namespace Assets._Asteroids.Logic.IAP.Products
{
    public interface IPurchaseProduct
    {
        string ProductId {get; }
        ProductType ProductType {get; }
        void OnPurchased();
    }
}