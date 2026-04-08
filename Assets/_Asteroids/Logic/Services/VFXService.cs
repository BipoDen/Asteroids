using Assets._Asteroids.Logic.Constants;
using UnityEngine;

namespace Assets._Asteroids.Logic.Services
{
    public class VFXService : IVFXService
    {
        private GameObject _explosionPrefab;
        private GameObject _projectileFlashPrefab;
        private GameObject _laserFlashPrefab;

        public void Initialize(GameObject explosionPrefab, GameObject projectileFlashPrefab, GameObject laserFlashPrefab)
        {
            _explosionPrefab = explosionPrefab;
            _projectileFlashPrefab = projectileFlashPrefab;
            _laserFlashPrefab = laserFlashPrefab;
        }

        public void CreateExplosion(Transform transform)
        {
            var effect = Object.Instantiate(_explosionPrefab, transform.position, transform.rotation);
            effect.transform.localScale = transform.localScale;
            effect.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            Object.Destroy(effect, AnimationsConstants.EXPLOSION_ANIM_TIME);
        }

        public void CreateProjectileFlash(Transform transform)
        {
            var effect = Object.Instantiate(_projectileFlashPrefab, transform);
            Object.Destroy(effect, AnimationsConstants.PROJECTILE_FLASH_TIME);
        }

        public void CreateLaserFlash(Transform transform)
        {
            var effect = Object.Instantiate(_laserFlashPrefab, transform);
            Object.Destroy(effect, AnimationsConstants.LASER_FLASH_TIME);
        }
    }
}