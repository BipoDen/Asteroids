using Assets._Asteroids.Logic.Gameplay;
using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.SaveProviders
{
    public interface ISaveProvider
    {
        public UniTask Initialize();
        bool IsAvailable();
        UniTask Save(SaveData data);
        UniTask<SaveData> Load();
    }
}