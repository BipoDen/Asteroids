using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.Entities.Player
{
    public class SpaceshipFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly Transform _startPosition;

        public SpaceshipFactory(IInstantiator instantiator, [Inject(Id = "StartPosition")] Transform startPosition)
        {
            _instantiator =  instantiator;
            _startPosition = startPosition;
        }

        public SpaceshipController CreatePlayer(SpaceshipController playerPrefab)
        {
            var player = _instantiator.InstantiatePrefabForComponent<SpaceshipController>(
                playerPrefab, _startPosition.position, Quaternion.identity, null);

            return player;
        }
    }
}