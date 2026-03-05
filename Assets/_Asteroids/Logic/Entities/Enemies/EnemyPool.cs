using Zenject;

namespace Assets._Asteroids.Logic.Entities.Enemies
{
    public class EnemyPool<T> : MemoryPool<T> where T : BaseEnemy
    {
        protected override void OnCreated(T item)
        {
            base.OnCreated(item);
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(T item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(T item)
        {
            base.OnDespawned(item);
            item.gameObject.SetActive(false);
        }
    }
}