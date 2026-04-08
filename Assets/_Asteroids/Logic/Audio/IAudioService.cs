using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Audio
{
    public interface IAudioService
    {
        public void Initialize(AudioController audioController);
        public void PlayProjectileShotAudio();
        public void PlayLaserShotAudio();
        public void PlayExplosionAudio(float volume);
        public void PlayBackgroundMusic();
    }
}