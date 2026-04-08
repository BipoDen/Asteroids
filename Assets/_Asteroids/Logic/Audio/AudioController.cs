using UnityEngine;

namespace Assets._Asteroids.Logic.Audio
{
    public class AudioController : MonoBehaviour, IAudioController
    {
        [SerializeField] private AudioSource _projectileAudio;
        [SerializeField] private AudioSource _laserAudio;
        [SerializeField] private AudioSource _explodeAudio;
        [SerializeField] private AudioSource _backgroundAudio;
        
        public void PlayProjectileShootAudio()
        {
            _projectileAudio.Play();
        }

        public void PlayLaserShootAudio()
        {
            _laserAudio.Play();
        }

        public void PlayExplosionAudio(float volume)
        {
            _explodeAudio.volume = volume;
            _explodeAudio.Play();
        }

        public void PlayBackgroundMusic()
        {
            _backgroundAudio.Play();
        }
    }
}