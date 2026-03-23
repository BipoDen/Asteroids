using System;

namespace Assets._Asteroids.Logic.RemoteConfig.Configs.Enemies
{
    [Serializable]
    public class AsteroidsConfig
    {
        public float SpawnDelay;
        public float AsteroidSpeed;
        public int FragmentCount;
        public int ScorePerKill;
    }
}