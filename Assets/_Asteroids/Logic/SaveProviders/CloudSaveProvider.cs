using System;
using System.Collections.Generic;
using Assets._Asteroids.Logic.Constants;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using Zenject;
using SaveData = Assets._Asteroids.Logic.Gameplay.SaveData;

namespace Assets._Asteroids.Logic.SaveProviders
{
    public class CloudSaveProvider : ISaveProvider, IInitializable
    {
        private const string SAVE_KEY = SaveConstants.CLOUD_SAVE_KEY;
        public async void Initialize()
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");
            }
        }
        
        public bool IsAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public async UniTask Save(SaveData data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var dict = new Dictionary<string, object>
                {
                    { SAVE_KEY, json }
                };
                await CloudSaveService.Instance.Data.Player.SaveAsync(dict);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Cloud Save failed: {e.Message}");
            }
        }

        public async UniTask<SaveData> Load()
        {
            try
            {
                var result = await CloudSaveService.Instance.Data.Player
                    .LoadAsync(new HashSet<string> { SAVE_KEY });

                if (result.TryGetValue(SAVE_KEY, out var item))
                {
                    var json = item.Value.GetAsString();
                    return JsonConvert.DeserializeObject<SaveData>(json);
                }
                
                return new SaveData();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Cloud Load failed: {e.Message}");
            }
            return null;
        }
    }
}