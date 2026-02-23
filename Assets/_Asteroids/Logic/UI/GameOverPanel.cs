using System;
using Assets._Asteroids.Logic.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets._Asteroids.Logic.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _restartButton;
        
        private GameState _gameState;
        private ScoreService _scoreService;
        
        [Inject]
        public void Construct(GameState gameState, ScoreService scoreService)
        {
            _gameState = gameState;
            _scoreService = scoreService;
        }

        private void Awake()
        {
            _restartButton.onClick.AddListener(RestartGame);
        }

        public void ShowScore()
        {
            _scoreText.text = $"Your score: {_scoreService.Score.ToString()}";
        }

        private void RestartGame()
        {
            _gameState.RestartGame();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(RestartGame);
        }
    }
}