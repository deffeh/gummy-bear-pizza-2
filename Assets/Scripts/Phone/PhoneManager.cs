using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace Phone
{
    public class PhoneManager : MonoBehaviour
    {
        public AudioClip PickUp;
        public AudioClip PutDown;
        public AudioClip Click;
        public AudioClip Reset;
        public AudioClip Multiplier;
        public AudioClip GainFatigue;
        public AudioSource Source;
        
        public TextMeshProUGUI TextPrefab;
        public static PhoneManager Instance;
        [SerializeField] private RectTransform phoneRect;        
        [SerializeField] private RectTransform screen;           
        // make sure the order of reel prefabs is [0]reset multiplier, [1]gain multiplier, [2]gain energy, [3]do nothing
        [SerializeField] private GameObject[] reelPrefabs;       
        [SerializeField] private float animationDuration = 1.0f;
        [SerializeField] private Vector2 activePosition;         
        [SerializeField] private int verticalOffset = -1610;
        [SerializeField] private int maxQueueSize = 3;
        [SerializeField] private float swipeCooldown = 0.3f;
        [SerializeField] private Button raiseButton;
        [SerializeField] private Button lowerButton;
        [SerializeField] private InactiveScreen inactiveScreen;
        [SerializeField] private GameObject _hand;
        [SerializeField] private Image statusBar;
        [SerializeField] private TMP_Text statusBarTimeText;

        private bool _isSwiped = false;
        private bool _isActive = false;
        private Vector2 _originalPosition;
        private Queue<Reel> _reelQueue = new Queue<Reel>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (phoneRect == null)
                phoneRect = GetComponent<RectTransform>();
            
            _originalPosition = phoneRect.anchoredPosition;

            raiseButton.onClick.AddListener(() =>
            {
                if (GameManager.Instance == null || GameManager.Instance.RoundOver) { return; }
                raiseButton.interactable = false;
                lowerButton.interactable = true;
                OpenPhone();
            });
            
            lowerButton.onClick.AddListener(() =>
            {
                lowerButton.interactable = false;
                raiseButton.interactable = true;
                ClosePhone();
            });
            
            statusBar.enabled = false;
            statusBarTimeText.enabled = false;
        }
        
        public void Update()
        {
            string time = GameManager.Instance.GetTime();
            if (statusBarTimeText) statusBarTimeText.text = time + " PM";
        }

        public bool IsActive()
        {
            return _isActive; 
        }

        public void OpenPhone()
        {
            phoneRect.DOAnchorPos(activePosition, animationDuration).SetEase(Ease.OutCubic);

            ClearAllReels();
            
            for (int i = 0; i < maxQueueSize; i++) 
                AddReel();
            
            _isActive = true;
            
            inactiveScreen.Hide();
            
            statusBar.enabled = true;
            statusBarTimeText.enabled = true;
            Source.clip = PickUp;
            Source.Play();
        }

        public void ClosePhone()
        {
            inactiveScreen.Show();

            _isActive = false;
            
            phoneRect.DOAnchorPos(_originalPosition, animationDuration).SetEase(Ease.OutCubic);
            
            ClearAllReels();
            
            statusBar.enabled = false;
            statusBarTimeText.enabled = false;
            Source.clip = PutDown;
            Source.Play();
        }

        public void Swipe()
        {
            if (_isSwiped || !_isActive) return;
            _hand.SetActive(false);
            _isSwiped = true;
            
            StartCoroutine(SwipeCooldownRoutine());
            
            AddReel();
            
            foreach (Reel reel in _reelQueue)
            {
                if (reel != null)
                {
                    RectTransform reelRect = reel.GetComponent<RectTransform>();
                    if (reelRect != null)
                    {
                        Vector2 newPos = reelRect.anchoredPosition + new Vector2(0, -verticalOffset);
                        reelRect.DOAnchorPos(newPos, animationDuration).SetEase(Ease.OutCubic);
                    }
                }
            }
            
            Reel reelToDestroy = null;
            if (_reelQueue.Count > 0)
            {
                reelToDestroy = _reelQueue.Dequeue();
            }
            
            if (reelToDestroy != null)
            {
                RectTransform reelRect = reelToDestroy.GetComponent<RectTransform>();
                if (reelRect != null)
                {
                    Vector2 newPos = reelRect.anchoredPosition + new Vector2(0, -verticalOffset);
                    reelRect.DOAnchorPos(newPos, animationDuration)
                        .SetEase(Ease.OutCubic)
                        .OnComplete(() => {
                            if (reelToDestroy != null)
                                Destroy(reelToDestroy.gameObject);
                        });
                }
            }
            
            // Add time to timer
            GameManager.Instance._gameTimer -= Reel.timeLostPerReel;
            
            // Activate reel effect
            _reelQueue.Peek()?.OnActivate();
            Source.clip = Click;
            Source.Play();
        }

        private IEnumerator SwipeCooldownRoutine()
        {
            yield return new WaitForSeconds(swipeCooldown);
            _isSwiped = false;
        }

        public static void PlaySound(int sound)
        {
            switch (sound)
            {
                case 0:
                    Instance.Source.PlayOneShot(Instance.Reset);
                    break;
                case 1:
                    Instance.Source.PlayOneShot(Instance.Multiplier);
                    break;
                case 2:
                    Instance.Source.PlayOneShot(Instance.GainFatigue);
                    break;
            }
        }
        
        private void AddReel()
        {
            GameObject reelPrefab = GetRandomReelPrefab();
            if (reelPrefab == null) return;

            GameObject reelObj = Instantiate(reelPrefab, screen, false);
            Reel newReel = reelObj.GetComponent<Reel>();
            
            if (newReel == null)
            {
                Destroy(reelObj);
                return;
            }
            
            newReel.Initialize();

            RectTransform reelRect = newReel.GetComponent<RectTransform>();
            if (reelRect != null)
            {
                float yPos = verticalOffset * (_reelQueue.Count);
                reelRect.anchoredPosition = new Vector2(0, yPos);
            }

            _reelQueue.Enqueue(newReel);
        }

        private GameObject GetRandomReelPrefab()
        {
            if (reelPrefabs == null || reelPrefabs.Length == 0)
            {
                return null;
            }

            int reelType = GetWeightedRandomReelIndex();
            // make sure the order of reel prefabs is [0]reset multiplier, [1]gain multiplier, [2]gain energy, [3]do nothing
            return reelPrefabs[reelType];
        }

        private int GetWeightedRandomReelIndex()
        {
            // make sure the order of reel prefabs is [0]reset multiplier, [1]gain multiplier, [2]gain energy, [3]do nothing
            float r = Random.value;
            if (r < 0.05f) return 0; // 5% for reset multipier
            if (r < 0.15) return 1; // 10% for gain multiplier
            if (r < 0.40) return 2; // 25% for gain energy
            return 3; // 60% for do nothing
        }
        
        private void ClearAllReels()
        {
            while (_reelQueue.Count > 0)
            {
                Reel reel = _reelQueue.Dequeue();
                if (reel != null)
                    Destroy(reel.gameObject);
            }
        }
    }
}