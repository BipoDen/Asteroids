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
        
        private PlayerDataProvider _dataProvider;
        private ISaveService _saveService;

        public NoAdsProduct(PlayerDataProvider playerDataProvider, ISaveService saveService)
        {
            _dataProvider = playerDataProvider;
            _saveService = saveService;
        }
        
        public void OnPurchased()
        {
            _dataProvider.SaveData.IsAdDisabled = true;
            _saveService.Save(_dataProvider.SaveData);
        }
    }
}