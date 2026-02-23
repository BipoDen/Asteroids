using Assets._Asteroids.Logic.Weapon;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Factory
{
    public class ProjectileFactory
    {
        private ProjectilePool _pool;
        
        [Inject]
        public void Construct(ProjectilePool pool)
        {
            _pool = pool;
        }

        public ProjectileView Create(Transform startPosition, float lifeTime, float speed)
        {
            var projectile = _pool.Spawn();
            projectile.Init(startPosition,  lifeTime, speed);

            projectile.OnDied += Despawn;
            
            void Despawn()
            {
                _pool.Despawn(projectile);
            }
            
            return projectile;
        }
    }
}