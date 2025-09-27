// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;
// using UnityEngine.UI;
//
// namespace Reels
// {
//     public class Phone : MonoBehaviour
//     {
//         [SerializeField] private Button raisePhoneButton;
//         [SerializeField] private Vector2 activePosition;
//         [SerializeField] private float animationDuration = 0.5f;
//         [SerializeField] private int _verticalOffset = -1610;
//             
//         [SerializeField] private PhoneManager phoneManager;
//         [SerializeField] private RectTransform screen; // The screen where reels will be added
//
//         
//         private Vector2 _originalPosition;
//         private RectTransform _rectTransform;
//         
//         private void Awake()
//         {
//             _rectTransform = GetComponent<RectTransform>();
//         }
//
//         public void OpenPhone()
//         {
//             _originalPosition = _rectTransform.anchoredPosition;
//             _rectTransform.DOAnchorPos(activePosition, animationDuration).SetEase(Ease.OutCubic);
//
//             if (raisePhoneButton != null)
//                 raisePhoneButton.interactable = false;
//
//             // Create and display the first reel
//             Reel firstReel = phoneManager.Swipe();
//             if (firstReel != null)
//             {
//                 QueueReel(firstReel);
//             }
//         }
//
//
//         public void Swipe()
//         {
//             
//         }
//
//         public void ClosePhone()
//         {
//             transform.DOMove(_originalPosition, animationDuration).SetEase(Ease.OutCubic);
//         }
//
//         public void QueueReel(Reel reel)
//         {
//             
//         }
//     }
// }
