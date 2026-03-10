
namespace Assets._Asteroids.Logic.Services
{
    public class StatsService : IStatsService
    {
        public int PrimaryCount { get; private set; }
        public int SecondaryCount { get; private set; }
        public int AsteroidsKillCount { get; private set; }
        public int UFOKillCount { get; private set; }
        
        public void AddPrimaryShot()
        {
            PrimaryCount++;
        }

        public void AddSecondaryShot()
        {
            SecondaryCount++;
        }

        public void AddAsteroidKill()
        {
            AsteroidsKillCount++;
        }

        public void AddUFOKill()
        {
            UFOKillCount++;
        }
    }
}