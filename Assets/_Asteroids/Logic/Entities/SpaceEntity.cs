using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using UnityEngine;

namespace Assets._Asteroids.Logic.Entities
{
    public class SpaceEntity : MonoBehaviour
    {
        protected SpaceScreen _spaceScreen;

        protected void TryTeleport()
        {
            transform.position = _spaceScreen.WrapPosition(transform.position);
        }
    }
}