using System;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.RemoteConfig
{
    public class FirebaseRemoteConfigProvider : IInitializable, IRemoteConfig
    {
        public void Initialize()
        {
            FetchDataAsync().Forget();
        }

        public T GetRemoteConfig<T>()
        {
            var json   = FirebaseRemoteConfig.DefaultInstance.GetValue(typeof(T).Name).StringValue;
            var config = JsonConvert.DeserializeObject<T>(json);
            return config;
        }

        private async UniTask FetchDataAsync()
        {
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;

            await remoteConfig.FetchAsync(TimeSpan.Zero).AsUniTask();

            if (remoteConfig.Info.LastFetchStatus != LastFetchStatus.Success)
                throw new Exception("Remote config cant be fetched");

            await remoteConfig.ActivateAsync().AsUniTask();
        }
    }
}