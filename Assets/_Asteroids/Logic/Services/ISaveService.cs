using System;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.SaveProviders;
using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Services
{
    public interface ISaveService
    {
        UniTask Save(SaveData data);
        UniTask<SaveData> Load();
        public event Action<SaveConflictResolveRequest> OnConflictDetected;
    }
}