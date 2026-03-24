using System;
using System.Collections.Generic;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.IAP.Products;
using Assets._Asteroids.Logic.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

namespace Assets._Asteroids.Logic.IAP
{
    public class UnityPurchasingService : IInitializable, IPurchasingService, IDisposable
    {
        private StoreController _storeController;
        private readonly Dictionary<string, IPurchaseProduct> _products = new();
        private bool _isInitialized;
        
        private UniTaskCompletionSource<bool> _purchaseTcs;
        
        public UnityPurchasingService(List<IPurchaseProduct> products)
        {
            foreach (var product in products)
                _products[product.ProductId] = product;
        }

        public void Initialize()
        {
            InitializeIAP().Forget();
        }

        private async UniTask InitializeIAP()
        {
            _storeController = UnityIAPServices.StoreController();
            
            _storeController.OnProductsFetched += OnProductsFetched;
            _storeController.OnPurchasesFetched += OnPurchasesFetched;
            _storeController.OnProductsFetchFailed += OnProductsFetchedFailed;
            _storeController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;
            _storeController.OnStoreDisconnected += OnStoreDisconnected;
            _storeController.OnPurchasePending += OnPurchasePending;
            _storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            _storeController.OnPurchaseFailed += OnPurchaseFailed;
            _storeController.OnPurchaseDeferred += OnPurchaseDeferred;
            
            await _storeController.Connect();

            var initialProductToFetch = BuildProductDefinitions();
            _storeController.FetchProducts(initialProductToFetch);
        }

        private List<ProductDefinition> BuildProductDefinitions()
        {
            var initialProductDefinitions = new List<ProductDefinition>();

            foreach (var product in _products.Values)
            {
                initialProductDefinitions.Add(new ProductDefinition(product.ProductId, product.ProductType));
            }
            
            return initialProductDefinitions;
        }

        private void OnPurchasePending(PendingOrder order)
        {
            _storeController.ConfirmPurchase(order);
        }
        
        private void OnProductsFetched(List<Product> products)
        {
            _storeController.FetchPurchases();
        }
        
        private void OnProductsFetchedFailed(ProductFetchFailed reason)
        {
            Debug.Log($"Failed to fetch products: {reason}");
        }
        
        private void OnPurchasesFetched(Orders orders)
        {
            _isInitialized = true;
        }
        
        private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription reason)
        {
            Debug.Log($"Failed to fetch purchases: {reason}");
        }
        
        private void OnStoreDisconnected(StoreConnectionFailureDescription description)
        {
            Debug.Log($"Initialization/Connection failed: {description}");
        }
        
        private void OnPurchaseConfirmed(Order order)
        {
            if (order?.Info?.PurchasedProductInfo != null && order.Info.PurchasedProductInfo.Count > 0)
            {
                string productId = order.Info.PurchasedProductInfo[0].productId;

                if(_products.TryGetValue(productId, out var product))
                    product.OnPurchased();
                
                _purchaseTcs?.TrySetResult(true);
            }
        }
        
        private void OnPurchaseFailed(FailedOrder failedOrder)
        {
            if (failedOrder?.Info?.PurchasedProductInfo == null || failedOrder.Info.PurchasedProductInfo.Count == 0)
            {
                Debug.Log($"Failed to purchase order: {failedOrder}");
                return;
            }
            
            var productId = failedOrder.Info.PurchasedProductInfo[0].productId;
            var reason = failedOrder.FailureReason;
            var message = failedOrder.Details;
            
            Debug.Log($"Failed to purchase order: {productId}. Reason: {reason}. Message: {message}");
            
            _purchaseTcs?.TrySetResult(false);
        }

        private void OnPurchaseDeferred(DeferredOrder deferredOrder)
        {
            Debug.Log($"Purchase deferred: {deferredOrder}");
        }

        public async UniTask<bool> MakePurchaseAsync(string productId)
        {
            if (!_isInitialized)
            {
                Debug.Log("IAP is not initialized");
                return false;
            }
            if (!_products.ContainsKey(productId)) return false;
            
            _purchaseTcs = new UniTaskCompletionSource<bool>();
            _storeController.PurchaseProduct(productId);
            
            return await _purchaseTcs.Task;
        }

        public void Dispose()
        {
            _storeController.OnProductsFetched -= OnProductsFetched;
            _storeController.OnPurchasesFetched -= OnPurchasesFetched;
            _storeController.OnProductsFetchFailed -= OnProductsFetchedFailed;
            _storeController.OnPurchasesFetchFailed -= OnPurchasesFetchFailed;
            _storeController.OnStoreDisconnected -= OnStoreDisconnected;
            _storeController.OnPurchasePending -= OnPurchasePending;
            _storeController.OnPurchaseConfirmed -= OnPurchaseConfirmed;
            _storeController.OnPurchaseFailed -= OnPurchaseFailed;
            _storeController.OnPurchaseDeferred -= OnPurchaseDeferred;
        }
    }
}