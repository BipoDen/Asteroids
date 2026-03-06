using Assets._Asteroids.Logic.Gameplay;

namespace Assets._Asteroids.Logic.Services
{
    public interface ISaveService
    {
        void Save(SaveData data);
        SaveData Load();
    }
}