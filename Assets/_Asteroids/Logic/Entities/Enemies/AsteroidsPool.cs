using Zenject;

namespace Assets._Asteroids.Logic.Entities.Enemies
{
    public class AsteroidsPool : MemoryPool<AsteroidEnemy>
    {
        protected override void OnCreated(AsteroidEnemy item)
        {
            base.OnCreated(item);
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(AsteroidEnemy item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(AsteroidEnemy item)
        {
            base.OnDespawned(item);
            item.gameObject.SetActive(false);
        }
    }
}