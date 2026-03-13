using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets._Asteroids.Logic.Addressable
{
    public interface IAssetLoader
    {
        UniTask<T> LoadAsync<T>(string key);
        //UniTask<GameObject> InstantiateAsync(string key, Transform parent = null);
        void Release(string key);
    }
}