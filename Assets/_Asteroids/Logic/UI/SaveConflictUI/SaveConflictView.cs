using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Asteroids.Logic.UI.SaveConflictUI
{
    public class SaveConflictView : MonoBehaviour
    {
        [SerializeField] private Button _localSaveButton;
        [SerializeField] private Button _cloudSaveButton;

        [SerializeField] private TextMeshProUGUI _localText;
        [SerializeField] private TextMeshProUGUI _cloudText;
        
        public Button.ButtonClickedEvent OnLocalSaveClick => _localSaveButton.onClick;
        public Button.ButtonClickedEvent OnCloudSaveClick => _cloudSaveButton.onClick;
        
        public void SetLocalText(string lastModifiedTime)
        {
            _localText.text = $"Last Modified: {lastModifiedTime}";
        }

        public void SetCloudText(string lastModifiedTime)
        {
            _cloudText.text = $"Last Modified: {lastModifiedTime}";
        }
    }
}