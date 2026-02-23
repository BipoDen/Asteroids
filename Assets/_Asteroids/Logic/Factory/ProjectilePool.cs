using Assets._Asteroids.Logic.Weapon;
using Zenject;

namespace Assets._Asteroids.Logic.Factory
{
    public class ProjectilePool : MemoryPool<ProjectileView>
    {
        protected override void OnCreated(ProjectileView item)
        {
            base.OnCreated(item);
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(ProjectileView item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(ProjectileView item)
        {
            base.OnDespawned(item);
            item.gameObject.SetActive(false);
        }
    }
}