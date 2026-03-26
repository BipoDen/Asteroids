using System;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.SaveProviders;
using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Services
{
    public class SaveService : ISaveService
    {
        private LocalSaveProvider _localSaveProvider;
        private CloudSaveProvider _cloudSaveProvider;
        
        private SaveData _saveData;

        public event Action<SaveConflictResolveRequest> OnConflictDetected;

        public SaveService(LocalSaveProvider localSaveProvider, CloudSaveProvider cloudSaveProvider)
        {
            _localSaveProvider = localSaveProvider;
            _cloudSaveProvider = cloudSaveProvider;
        }

        public async UniTask Save(SaveData data)
        {
            data.LastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _localSaveProvider.Save(data);

            if (_cloudSaveProvider.IsAvailable())
                await _cloudSaveProvider.Save(data);
            
        }

        public async UniTask<SaveData> Load()
        {
            if (!_cloudSaveProvider.IsAvailable())
            {
                _saveData = await _localSaveProvider.Load();
                return _saveData;
            }

            try
            {
                await _cloudSaveProvider.InitializeAsync();
            }
            catch
            {
                _saveData = await _localSaveProvider.Load();
                return _saveData;
            }

            (SaveData localSave, SaveData cloudSave) =
                await UniTask.WhenAll(_localSaveProvider.Load(), _cloudSaveProvider.Load());
            
            var activeSave = await ResolveConflict(localSave, cloudSave);

            return activeSave;
        }

        private async UniTask<SaveData> ResolveConflict(SaveData localSave, SaveData cloudSave)
        {
            if (localSave == null) return cloudSave;
            if (cloudSave == null) return localSave;
            
            if (cloudSave.LastModified >= localSave.LastModified)
                return cloudSave;
            
            var request = new SaveConflictResolveRequest
            {
                LocalSave = localSave,
                CloudSave = cloudSave
            };
            
            OnConflictDetected?.Invoke(request);
            
            var chosen = await request.CompletionSource.Task;
            
            _localSaveProvider.Save(chosen);
            await _cloudSaveProvider.Save(chosen);

            return chosen;
        }
    }
}