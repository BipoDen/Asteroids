using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.RemoteConfig
{
    public interface IRemoteConfig
    {
        public UniTask Initialize();
        T GetRemoteConfig<T>();
    }
}