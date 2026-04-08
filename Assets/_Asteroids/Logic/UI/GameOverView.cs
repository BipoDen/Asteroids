using DG.Tweening;
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
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Image _background;

        public Button.ButtonClickedEvent OnRestart => _restartButton.onClick;
        public Button.ButtonClickedEvent OnAdClick => _AdClickButton.onClick;
        
        private const float FADE_DURATION = .5f;
        private const float FADE_IN_VALUE = .8F;
        private const float FADE_OUT_VALUE = 0f;
        private const float SCALE_DURATION = .75f;
        private const float SCALE_IN_VALUE = 1f;
        
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

        public void ShowPanel(int maxScore, int score)
        {
            gameObject.SetActive(true);
            SetMaxScore(maxScore, score);
            ShowScore(score);
            _gameOverPanel.transform.localScale = Vector2.zero;
            _background.DOFade(FADE_IN_VALUE, FADE_DURATION);
            _gameOverPanel.transform.DOScale(SCALE_IN_VALUE, SCALE_DURATION).SetEase(Ease.OutBack);
        }

        public void HidePanel()
        {
            _gameOverPanel.transform.localScale = Vector3.one;
            _background.DOFade(FADE_OUT_VALUE, FADE_DURATION);
            _gameOverPanel.transform.DOScale(Vector2.zero, SCALE_DURATION).SetEase(Ease.InBack);
            gameObject.SetActive(false);
        }
    }
}