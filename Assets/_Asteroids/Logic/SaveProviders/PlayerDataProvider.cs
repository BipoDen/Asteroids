using Assets._Asteroids.Logic.Gameplay;

namespace Assets._Asteroids.Logic.SaveProviders
{
    public class PlayerDataProvider
    {
        public SaveData SaveData {get; private set;}
        
        public void SetSaveData(SaveData saveData)
        {
            SaveData = saveData;
        }
    }
}