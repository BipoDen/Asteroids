namespace Assets._Asteroids.Logic.Analytics
{
    public interface IAnalyticsService
    {
        public void OnStartGameEvent();
        public void OnGameOverEvent(int primaryCount, int secondaryCount, int destroyedAsteroidsCount, int destroyedUFOsCount);
        public void OnLaserUsingEvent();
    }
}