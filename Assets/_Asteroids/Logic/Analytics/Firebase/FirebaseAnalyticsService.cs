using Firebase.Analytics;
using UnityEngine;

namespace Assets._Asteroids.Logic.Analytics.Firebase
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        public void OnStartGameEvent()
        {
            Debug.Log("FirebaseStartEvent");
            FirebaseAnalytics.LogEvent(AnalyticsConstants.GAME_STARTED, new Parameter("Start", "StartGame"));
        }

        public void OnGameOverEvent(int primaryCount, int secondaryCount, int destroyedAsteroidsCount, int destroyedUFOsCount)
        {
            Debug.Log("FirebaseEndEvent");
            FirebaseAnalytics.LogEvent(AnalyticsConstants.GAME_COMPLETED, 
                new Parameter("primary_count", primaryCount),
                new Parameter("secondary_count", secondaryCount),
                new Parameter("destroyed_asteroids_count", destroyedAsteroidsCount),
                new Parameter("destroyed_ufos_count", destroyedUFOsCount));
        }

        public void OnLaserUsingEvent()
        {
            Debug.Log("FirebaseLaserUsingEvent");
            FirebaseAnalytics.LogEvent(AnalyticsConstants.GAME_COMPLETED, new Parameter("Laser", "UsingLaser"));
        }
    }
}