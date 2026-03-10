using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Analytics.Firebase
{
    public class FirebaseInitializer : IInitializable
    {
        public void Initialize()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(OnDependencyStatusRecieved);
        }

        private async UniTask OnDependencyStatusRecieved(Task<DependencyStatus> task)
        {
            try
            {
                if(!task.IsCompletedSuccessfully)
                    throw new Exception("Could not resolve all Firebase dependencies", task.Exception);
                
                var status = task.Result;
                if(status != DependencyStatus.Available)
                    throw new Exception($"Could not resolve all Firebase dependencies: {status}");
                
                Debug.Log("Firebase initialized successfully");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}