using System;

namespace Assets._Asteroids.Logic.RemoteConfig.Configs.Enemies
{
    [Serializable]
    public class AsteroidsConfig
    {
        public float SpawnDelay;
        public float AsteroidSpeed;
        public float FragmentSpeed;
        public int FragmentCount;
        public int ScorePerKill;
        public int ScorePerFragmentKill;
        public float AsteroidSize;
        public float FragmentSize;
    }
}