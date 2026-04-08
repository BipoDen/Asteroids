using Cysharp.Threading.Tasks;

namespace Assets._Asteroids.Logic.Audio
{
    public class AudioService : IAudioService
    {
        private AudioController _audioController;
        
        public void Initialize(AudioController audioController)
        {
            _audioController = audioController;
        }

        public void PlayProjectileShotAudio()
        {
            _audioController.PlayProjectileShootAudio();
        }

        public void PlayLaserShotAudio()
        {
            _audioController.PlayLaserShootAudio();
        }

        public void PlayExplosionAudio(float volume)
        {
            _audioController.PlayExplosionAudio(volume);
        }

        public void PlayBackgroundMusic()
        {
            _audioController.PlayBackgroundMusic();
        }
    }
}