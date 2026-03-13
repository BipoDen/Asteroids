using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets._Asteroids.Logic.Addressable
{
    public class BaseAssetLoader : IAssetLoader, IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> _cachedHandles = new();
        
        public async UniTask<T> LoadAsync<T>(string key)
        {
            if (_cachedHandles.TryGetValue(key, out var cachedHandle))
                return (T)cachedHandle.Result;

            var handle = Addressables.LoadAssetAsync<T>(key);
            await handle.ToUniTask();
            
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load prefab: {handle.OperationException?.Message}");
                return default;
            }
            
            _cachedHandles[key] = handle;

            return handle.Result;
        }

        public void Release(string key)
        {
            if (_cachedHandles.TryGetValue(key, out var handle))
            {
                Addressables.Release(handle);
                _cachedHandles.Remove(key);
            }
        }

        public void Dispose()
        {
            foreach (var handle in _cachedHandles.Values)
                Addressables.Release(handle);
            _cachedHandles.Clear();
        }
    }
}