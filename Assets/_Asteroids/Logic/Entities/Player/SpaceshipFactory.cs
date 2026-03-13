using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Entities.Player
{
    public class SpaceshipFactory
    {
        private readonly DiContainer _container;
        private readonly Transform _startPosition;

        public SpaceshipFactory(DiContainer container, [Inject(Id = "StartPosition")] Transform startPosition)
        {
            _container = container;
            _startPosition = startPosition;
        }

        public SpaceshipController CreatePlayer(GameObject playerPrefab)
        {
            var player = _container.InstantiatePrefabForComponent<SpaceshipController>(
                playerPrefab, _startPosition.position, Quaternion.identity, null);
            
            _container.BindInterfacesAndSelfTo<SpaceshipController>()
                .FromInstance(player).AsSingle();
            
            return player;
        }
    }
}