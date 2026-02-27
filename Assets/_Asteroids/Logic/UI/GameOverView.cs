using System;
using Assets._Asteroids.Logic.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets._Asteroids.Logic.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _restartButton;

        public Button.ButtonClickedEvent OnRestart => _restartButton.onClick;
        
        public void ShowScore(int score)
        {
            _scoreText.text = $"Your score: {score.ToString()}";
        }
    }
}