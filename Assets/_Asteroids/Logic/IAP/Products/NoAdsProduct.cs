using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using UnityEngine.Purchasing;

namespace Assets._Asteroids.Logic.IAP.Products
{
    public class NoAdsProduct : IPurchaseProduct
    {
        public string ProductId => ProductsConstants.NO_ADS_PRODUCT;
        public ProductType ProductType => ProductType.NonConsumable;
        
        private SaveData _saveData;
        private ISaveService _saveService;

        public NoAdsProduct(SaveData saveData, ISaveService saveService)
        {
            _saveData = saveData;
            _saveService = saveService;
        }
        
        public void OnPurchased()
        {
            _saveData.IsAdDisabled = true;
            _saveService.Save(_saveData);
        }
    }
}