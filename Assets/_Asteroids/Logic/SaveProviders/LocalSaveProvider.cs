using Assets._Asteroids.Logic.Gameplay;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets._Asteroids.Logic.SaveProviders
{
    public class LocalSaveProvider : ISaveProvider
    {
        private const string SAVE_KEY = "PLAYER_DATA_LOCAL";

        public bool IsAvailable() => true;

        public async UniTask Save(SaveData data)
        {
            var json = JsonConvert.SerializeObject(data);
            
            PlayerPrefs.SetString(SAVE_KEY, json);
        }

        public async UniTask<SaveData> Load()
        {
            var json = PlayerPrefs.GetString(SAVE_KEY);
            
            var data = string.IsNullOrEmpty(json) 
                ? new SaveData()
                : JsonConvert.DeserializeObject<SaveData>(json);
            
            Save(data);
            Debug.Log("LocalSave Loaded");
            return data;
        }
    }
}