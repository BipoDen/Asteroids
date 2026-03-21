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
        private readonly IInstantiator _instantiator;
        private GameObject _prefab;

        public EnemyPool(IInstantiator instantiator, string groupName)
        {
            _instantiator = instantiator;
            _container = new GameObject(groupName).transform;
        }
        
        public void Initialize(T prefab, int initialSize)
        {
            _prefab = prefab.gameObject;
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
            return _instantiator.InstantiatePrefabForComponent<T>(_prefab, _container);
        }

        public void Dispose()
        {
            Object.Destroy(_container.gameObject);
        }
    }
}