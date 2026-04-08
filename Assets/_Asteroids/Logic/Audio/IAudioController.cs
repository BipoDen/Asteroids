namespace Assets._Asteroids.Logic.Audio
{
    public interface IAudioController
    {
        void PlayProjectileShootAudio();
        void PlayLaserShootAudio();
        void PlayExplosionAudio(float volume);
        void PlayBackgroundMusic();
    }
}