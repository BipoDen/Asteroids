using Zenject;

namespace Assets._Asteroids.Logic.Entities.Enemies
{
    public class UFOPool : MemoryPool<UFOEnemy>
    {
        protected override void OnCreated(UFOEnemy item)
        {
            base.OnCreated(item);
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(UFOEnemy item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(UFOEnemy item)
        {
            base.OnDespawned(item);
            item.gameObject.SetActive(false);
        }
    }
}