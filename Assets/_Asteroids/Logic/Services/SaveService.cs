using Assets._Asteroids.Logic.Gameplay;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets._Asteroids.Logic.Services
{
    public class SaveService : ISaveService
    {
        private const string SAVE_KEY = "PLAYER_DATA";
        public SaveData Data { get; private set; }

        public SaveService()
        {
            Load();
        }
        
        public void Save()
        {
            var json = JsonConvert.SerializeObject(Data);
            
            PlayerPrefs.SetString(SAVE_KEY, json);
        }

        public void Load()
        {
            var json = PlayerPrefs.GetString(SAVE_KEY);
            
            Data = string.IsNullOrEmpty(json) 
                ? new SaveData()
                : JsonConvert.DeserializeObject<SaveData>(json);
            
            Save();
        }
    }
}