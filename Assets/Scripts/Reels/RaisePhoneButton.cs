using UnityEngine;
using UnityEngine.UI;

namespace Reels
{
    public class RaisePhoneButton : MonoBehaviour
    {
        private Button _button;

        public void Start()
        {
            _button = GetComponent<Button>();
        }
        public void Disable()
        {
            if (_button != null) _button.interactable = false;
        }
    }
}
