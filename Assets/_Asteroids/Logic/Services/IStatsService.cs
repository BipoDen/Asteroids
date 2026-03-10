namespace Assets._Asteroids.Logic.Services
{
    public interface IStatsService
    {
        public void AddPrimaryShot();
        public void AddSecondaryShot();
        public void AddAsteroidKill();
        public void AddUFOKill();
    }
}