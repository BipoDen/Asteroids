using System;
using System.Collections.Generic;
using Assets._Asteroids.Logic.Weapon;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Assets._Asteroids.Logic.Factory
{
    public class ProjectilePool : IDisposable
    {
        private readonly Stack<ProjectileView> _pool = new();
        private readonly Transform _container;
        private readonly DiContainer _diContainer;
        private GameObject _prefab;

        public ProjectilePool(DiContainer diContainer, string groupName)
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

        public ProjectileView Spawn()
        {
            var projectile = _pool.Count > 0 ? _pool.Pop() : CreateNew();
            projectile.gameObject.SetActive(true);
            return projectile;
        }

        public void Despawn(ProjectileView projectile)
        {
            projectile.gameObject.SetActive(false);
            _pool.Push(projectile);
        }

        private ProjectileView CreateNew()
        {
            return _diContainer.InstantiatePrefabForComponent<ProjectileView>(_prefab, _container);
        }

        public void Dispose()
        {
            Object.Destroy(_container.gameObject);
        }
    }
}