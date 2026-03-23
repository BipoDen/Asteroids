using System;

namespace Assets._Asteroids.Logic.RemoteConfig.Configs.Weapons
{
    [Serializable]
    public class LaserWeaponConfig
    {
        public float LaserDuration;
        public float LaserDistance;
        public float LaserCooldown;
        public int MaxLaserCount;
    }
}