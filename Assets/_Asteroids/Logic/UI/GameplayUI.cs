using System;
using Assets._Asteroids.Logic.Factory;
using Assets._Asteroids.Logic.Services;
using Assets._Asteroids.Logic.Weapon;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets._Asteroids.Logic.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _secondaryCountText;
        [SerializeField] private GameOverPanel _gameOverPanel;
        
        private GameState _gameState;
        private ScoreService _scoreService;
        private IWeapon _secondaryWeapon;

        [Inject]
        public void Construct(ScoreService scoreService, [Inject(Id = "Secondary")] IWeapon secondaryWeapon, GameState gameState)
        {
            _scoreService = scoreService;
            _secondaryWeapon = secondaryWeapon;
            _gameState = gameState;
            _gameState.OnGameOver += ShowGameOverPanel;
        }

        private void Awake()
        {
            _scoreService.OnScoreChanged += ChangeScore;
            _secondaryWeapon.OnCountChanged += ChangeSecondaryCount;
        }

        private void ChangeScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void ChangeSecondaryCount(int count)
        {
            _secondaryCountText.text = $"Lasers left: {count.ToString()}";
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