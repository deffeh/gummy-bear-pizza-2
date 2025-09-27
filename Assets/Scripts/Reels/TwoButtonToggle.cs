using UnityEngine;
using UnityEngine.UI;

namespace Reels
{
    public class TwoButtonToggle : MonoBehaviour
    {
        [SerializeField] private Button otherButton;

        private Button _button;

        public void Start()
        {
            _button = GetComponent<Button>();

            if (!_button || !otherButton) return;
            
            if (otherButton.interactable == _button.interactable)
            {
                Debug.LogWarning("Only one button should start as active");
            }
        }
        
        public void OnButtonClicked()
        {
            Debug.Log("OnButtonClicked");
            
            if (!_button || !otherButton) return;
            
            _button.interactable = false;
            otherButton.interactable = true;

            Debug.Log(_button.interactable);
            Debug.Log(otherButton.interactable);
        }
    }
}
