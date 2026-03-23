namespace Assets._Asteroids.Logic.RemoteConfig
{
    public interface IRemoteConfig
    {
        T GetRemoteConfig<T>();
    }
}