using System;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.Gameplay;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Assets._Asteroids.Logic.UI
{
    public class GameplayUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _coordsText;
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _rotationText;
        [SerializeField] private TextMeshProUGUI _secondaryCountText;
        [SerializeField] private Slider _laserCooldown;

        public void ChangeScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void ChangeSecondaryCount(int count)
        {
            _secondaryCountText.text = $"Lasers left: {count.ToString()}";
        }

        public void ChangeSecondaryCooldown(float currentCooldown, float cooldown)
        {
            _laserCooldown.value = currentCooldown / cooldown;
        }

        public void ChangePlayerCoords(Vector2 coords, float speed)
        {
            _coordsText.text = $"{coords.x:F2}, {coords.y:F2}";
            _speedText.text = $"Speed: {speed:F2}";
        }

        public void ChangePlayerRotation(float zAngle)
        {
            _rotationText.text = $"{zAngle:F0}";
        }
    }
}