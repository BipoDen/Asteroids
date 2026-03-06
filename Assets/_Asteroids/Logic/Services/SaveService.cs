using Assets._Asteroids.Logic.Gameplay;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets._Asteroids.Logic.Services
{
    public class SaveService : ISaveService
    {
        private const string SAVE_KEY = "PLAYER_DATA";

        public SaveService()
        {
            Load();
        }
        
        public void Save(SaveData data)
        {
            var json = JsonConvert.SerializeObject(data);
            
            PlayerPrefs.SetString(SAVE_KEY, json);
        }

        public SaveData Load()
        {
            var json = PlayerPrefs.GetString(SAVE_KEY);
            
            var data = string.IsNullOrEmpty(json) 
                ? new SaveData()
                : JsonConvert.DeserializeObject<SaveData>(json);
            
            Save(data);

            return data;
        }
    }
}