using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Addressable
{
    public interface IAssetLoader
    {
        UniTask<T> LoadAsync<T>(string key) where T : UnityEngine.Object;
        void Release(string key);
    }
}