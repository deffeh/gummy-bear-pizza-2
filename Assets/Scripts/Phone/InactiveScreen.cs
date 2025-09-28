using TMPro;
using UnityEngine;

namespace Phone
{
    public class InactiveScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText;

        public void Update()
        {
            string time = GameManager.Instance.GetTime();
            if (timeText) timeText.text = time + " PM";
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
    }
}
