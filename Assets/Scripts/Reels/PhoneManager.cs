using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Reels
{
    public class PhoneManager : MonoBehaviour
    {
        [SerializeField] private RectTransform phoneRect;        
        [SerializeField] private RectTransform screen;           
        [SerializeField] private Button raisePhoneButton;

        [SerializeField] private GameObject[] reelPrefabs;       
        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField] private Vector2 activePosition;         
        [SerializeField] private int verticalOffset = -1610;
        [SerializeField] private int initialQueueSize = 3;

        private Vector2 _originalPosition;
        private Queue<Reel> _reelQueue;

        private void Awake()
        {
            if (phoneRect == null)
                phoneRect = GetComponent<RectTransform>();

            _originalPosition = phoneRect.anchoredPosition;
            _reelQueue = new Queue<Reel>();
        }

        public void OpenPhone()
        {
            phoneRect.DOAnchorPos(activePosition, animationDuration).SetEase(Ease.OutCubic);

            if (raisePhoneButton != null)
                raisePhoneButton.interactable = false;

            for (int i = 0; i < initialQueueSize; i++) AddReel();
        }

        public void ClosePhone()
        {
            phoneRect.DOAnchorPos(_originalPosition, animationDuration).SetEase(Ease.OutCubic);
        }

        public void Swipe()
        {
            AddReel();

            Vector2 nextPos = phoneRect.anchoredPosition + new Vector2(0, verticalOffset);
            phoneRect.DOAnchorPos(nextPos, animationDuration).SetEase(Ease.OutCubic);

            if (_reelQueue.Count > 3)
            {
                Reel oldReel = _reelQueue.Dequeue();
                Destroy(oldReel);
            }
        }

        private void AddReel()
        {
            GameObject reelPrefab = GetRandomReelPrefab();
            if (reelPrefab == null) return;

            GameObject reelObj = Instantiate(reelPrefab, screen, false);
            Reel newReel = reelObj.GetComponent<Reel>();
            
            newReel.Initialize();

            RectTransform reelRect = newReel.GetComponent<RectTransform>();
            if (reelRect != null)
            {
                int reelCount = screen.childCount - 1;
                reelRect.anchoredPosition = new Vector2(0, verticalOffset * reelCount);
            }

            _reelQueue.Enqueue(newReel);
        }

        private GameObject GetRandomReelPrefab()
        {
            if (reelPrefabs == null || reelPrefabs.Length == 0)
                return null;

            int index = Random.Range(0, reelPrefabs.Length);
            return reelPrefabs[index];
        }
    }
}
