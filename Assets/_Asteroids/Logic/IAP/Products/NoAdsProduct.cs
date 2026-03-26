using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.SaveProviders;
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

        public NoAdsProduct(PlayerDataProvider playerDataProvider, ISaveService saveService)
        {
            _saveData = playerDataProvider.SaveData;
            _saveService = saveService;
        }
        
        public void OnPurchased()
        {
            _saveData.IsAdDisabled = true;
            _saveService.Save(_saveData);
        }
    }
}