using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Assets._Asteroids.Logic.Entities.Enemies
{
    public class EnemyPool<T> : IDisposable where T : BaseEnemy
    {
        private readonly Stack<T> _pool = new();
        private readonly Transform _container;
        private readonly DiContainer _diContainer;
        private GameObject _prefab;

        public EnemyPool(DiContainer diContainer, string groupName)
        {
            _diContainer = diContainer;
            _container = new GameObject(groupName).transform;
        }
        
        public void Initialize(GameObject prefab, int initialSize)
        {
            _prefab = prefab;
            Debug.Log(_container);
            for (int i = 0; i < initialSize; i++)
            {
                var enemy = CreateNew();
                enemy.gameObject.SetActive(false);
                _pool.Push(enemy);
            }
        }

        public T Spawn()
        {
            var enemy = _pool.Count > 0 ? _pool.Pop() : CreateNew();
            enemy.gameObject.SetActive(true);
            return enemy;
        }

        public void Despawn(T enemy)
        {
            enemy.gameObject.SetActive(false);
            _pool.Push(enemy);
        }

        private T CreateNew()
        {
            return _diContainer.InstantiatePrefabForComponent<T>(_prefab, _container);
        }

        public void Dispose()
        {
            Object.Destroy(_container.gameObject);
        }
    }
}