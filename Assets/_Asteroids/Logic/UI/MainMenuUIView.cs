using UnityEngine;
using UnityEngine.UI;

namespace Assets._Asteroids.Logic.UI
{
    public class MainMenuUIView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _removeAdsButton;
        
        public Button.ButtonClickedEvent OnStartPlay => _playButton.onClick;
        public Button.ButtonClickedEvent OnRemoveAdsClick => _removeAdsButton.onClick;
        
        public void SetRemovingAdsInteractable(bool interactable)
        {
            _removeAdsButton.interactable = interactable;
        }
    }
}