using Assets._Asteroids.Logic.Gameplay;

namespace Assets._Asteroids.Logic.Services
{
    public interface ISaveService
    {
        SaveData Data { get; }
        void Save();
        void Load();
    }
}