using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Asteroids.Logic.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _maxScoreText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _AdClickButton;

        public Button.ButtonClickedEvent OnRestart => _restartButton.onClick;
        public Button.ButtonClickedEvent OnAdClick => _AdClickButton.onClick;
        
        public void ShowScore(int score)
        {
            _scoreText.text = $"Your score: {score.ToString()}";
        }

        public void SetMaxScore(int maxScore, int score)
        {
            if(maxScore > score)
                _maxScoreText.text = $"Max score: {maxScore.ToString()}";
            else
                _maxScoreText.text = "New Record!";
        }
        
        public void SetAdButtonInteractable(bool interactable)
        {
            _AdClickButton.interactable = interactable;
        }
    }
}