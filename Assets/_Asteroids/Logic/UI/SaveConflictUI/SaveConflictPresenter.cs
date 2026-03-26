using System;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.SaveProviders;
using Assets._Asteroids.Logic.Services;

namespace Assets._Asteroids.Logic.UI.SaveConflictUI
{
    public class SaveConflictPresenter : IDisposable
    {
        private ISaveService _saveService;
        private SaveData _localSaveData;
        private SaveData _cloudSaveData;
        private SaveConflictView _view;
        
        private SaveConflictResolveRequest _currentRequest;

        public SaveConflictPresenter(ISaveService saveService)
        {
            _saveService = saveService;
            _saveService.OnConflictDetected += ShowConflictWindow;
        }

        public void Initialize(SaveConflictView view)
        {
            _view = view;
            _view.gameObject.SetActive(false);
            _view.OnLocalSaveClick.AddListener(OnLocalSave);
            _view.OnCloudSaveClick.AddListener(OnCloudSave);
        }

        private void OnLocalSave()
        {
            _currentRequest.CompletionSource.TrySetResult(_localSaveData);
            _view.gameObject.SetActive(false);
        }

        private void OnCloudSave()
        {
            _currentRequest.CompletionSource.TrySetResult(_cloudSaveData);
            _view.gameObject.SetActive(false);
        }
        
        private void ShowConflictWindow(SaveConflictResolveRequest request)
        {
            _currentRequest = request;
            _localSaveData = request.LocalSave;
            _cloudSaveData = request.CloudSave;
            
            _view.SetLocalText(FormatTimestamp(_localSaveData.LastModified));
            _view.SetCloudText(FormatTimestamp(_cloudSaveData.LastModified));
            _view.gameObject.SetActive(true);
        }
        
        private string FormatTimestamp(long timestamp) =>
            DateTimeOffset
                .FromUnixTimeSeconds(timestamp)
                .LocalDateTime
                .ToString("dd.MM.yyyy HH:mm");
        
        public void Dispose()
        {
            _saveService.OnConflictDetected -= ShowConflictWindow;
            _view.OnLocalSaveClick.RemoveListener(OnLocalSave);
            _view.OnCloudSaveClick.RemoveListener(OnCloudSave);
        }
    }
}