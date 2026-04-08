using UnityEngine;

namespace Assets._Asteroids.Logic.Services
{
    public interface IVFXService
    {
        public void CreateExplosion(Transform transform);
        public void CreateProjectileFlash(Transform transform);
        public void CreateLaserFlash(Transform transform);
    }
}