using Assets._Asteroids.Logic.Addressable;
using Assets._Asteroids.Logic.Ads;
using Assets._Asteroids.Logic.Ads.UnityAds;
using Assets._Asteroids.Logic.Analytics;
using Assets._Asteroids.Logic.Analytics.Firebase;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.IAP;
using Assets._Asteroids.Logic.IAP.Products;
using Assets._Asteroids.Logic.RemoteConfig;
using Assets._Asteroids.Logic.Services;
using UnityEngine.Purchasing;
using Zenject;

namespace Assets._Asteroids.Logic.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveService>().To<SaveService>().FromNew().AsSingle();
            var saveService = Container.Resolve<ISaveService>();
            var data = saveService.Load();
            Container.Bind<SaveData>().FromInstance(data).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<FirebaseInitializer>().FromNew().AsSingle();
            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BaseAssetLoader>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<UnityAdsInitializer>().AsSingle();
            Container.BindInterfacesAndSelfTo<RewardedAds>().AsSingle();
            Container.BindInterfacesAndSelfTo<InterstitialAds>().AsSingle();
            Container.Bind<IAdService>().To<UnityAdsService>().AsSingle();
            Container.BindInterfacesTo<FirebaseRemoteConfigProvider>().AsSingle();
            
            InstallIAPs();
        }

        private void InstallIAPs()
        {
            Container.Bind<IPurchaseProduct>().To<NoAdsProduct>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<UnityPurchasingService>().AsSingle();
        }
    }
}