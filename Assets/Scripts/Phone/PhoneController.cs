using UnityEngine;
using UnityEngine.EventSystems;

namespace Phone
{
    public class PhoneSwipeHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float swipeThreshold = 100f;

        private PhoneManager _phoneManager;
        private Vector2 _startPos;

        public void Start()
        {
            _phoneManager = GetComponentInParent<PhoneManager>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_phoneManager.IsActive()) return;
            
            _startPos = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_phoneManager.IsActive()) return;
            
            Vector2 endPos = eventData.position;
            float swipeDelta = endPos.y - _startPos.y;

            if (Mathf.Abs(swipeDelta) > swipeThreshold)
            {
                _phoneManager.Swipe();
            }
        }
    }
}