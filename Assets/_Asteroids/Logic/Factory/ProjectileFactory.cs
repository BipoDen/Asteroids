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
            projectile.OnDied += Despawn;
            projectile.Init(startPosition,  lifeTime, speed);
            
            void Despawn()
            {
                projectile.OnDied -= Despawn;
                _pool.Despawn(projectile);
            }
            
            return projectile;
        }
    }
}