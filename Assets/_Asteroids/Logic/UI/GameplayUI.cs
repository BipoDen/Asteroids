using System;
using Assets._Asteroids.Logic.Entities.Player;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets._Asteroids.Logic.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _coordsText;
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _rotationText;
        [SerializeField] private TextMeshProUGUI _secondaryCountText;
        [SerializeField] private Slider _laserCooldown;
        [SerializeField] private GameOverPanel _gameOverPanel;
        
        private SpaceshipController _player;
        private GameState _gameState;
        private ScoreService _scoreService;
        private IWeapon _secondaryWeapon;

        [Inject]
        public void Construct(ScoreService scoreService, 
            SpaceshipController player, 
            [Inject(Id = "Secondary")] IWeapon secondaryWeapon, 
            GameState gameState)
        {
            _scoreService = scoreService;
            _player = player;
            _secondaryWeapon = secondaryWeapon;
            _gameState = gameState;
            _gameState.OnGameOver += ShowGameOverPanel;
        }

        private void Awake()
        {
            _scoreService.OnScoreChanged += ChangeScore;
            _secondaryWeapon.OnCountChanged += ChangeSecondaryCount;
            _secondaryWeapon.OnReloadTimeChanged += ChangeSecondaryCooldown;
            _player.OnMove += ChangePlayerCoords;
            _player.OnRotate += ChangePlayerRotation;
        }

        private void ChangeScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void ChangeSecondaryCount(int count)
        {
            _secondaryCountText.text = $"Lasers left: {count.ToString()}";
        }

        private void ChangeSecondaryCooldown(float curretCooldown, float cooldown)
        {
            _laserCooldown.value = curretCooldown / cooldown;
        }

        private void ChangePlayerCoords(Vector2 coords, float speed)
        {
            _coordsText.text = $"{coords.x:F2}, {coords.y:F2}";
            _speedText.text = $"Speed: {speed:F2}";
        }

        private void ChangePlayerRotation(float zAngle)
        {
            _rotationText.text = $"{zAngle:F0}";
        }

        private void ShowGameOverPanel()
        {
            _gameOverPanel.gameObject.SetActive(true);
            _gameOverPanel.ShowScore();
            _gameState.PauseGame();
        }

        private void OnDestroy()
        {
            _scoreService.OnScoreChanged -= ChangeScore;
            _secondaryWeapon.OnCountChanged -= ChangeSecondaryCount;
        }
    }
}