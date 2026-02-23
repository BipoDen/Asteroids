using System.Collections.Generic;
using Assets._Asteroids.Logic.Entities.Enemies;
using Assets._Asteroids.Logic.Services;

namespace Assets._Asteroids.Logic.Repository
{
    public class EnemyRepository
    {
        public readonly List<BaseEnemy> _enemies = new();

        private GameState _gameState;

        public EnemyRepository(GameState gameState)
        {
            _gameState = gameState;
        }
        
        public void RegisterEnemy(BaseEnemy enemy)
        {
            _enemies.Add(enemy);
        }

        public void UnregisterEnemy(BaseEnemy enemy)
        {
            _enemies.Remove(enemy);
        }
    }
}