using DG.Tweening;
using Phone;
using UnityEngine;
using UnityEngine.UI;

public class PhoneArrow : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTrans;
    [SerializeField] private CanvasGroup _canGrp;
    void Start()
    {
        int round = PlayerManager.Instance.round;
        if (round != 1)
        {
            gameObject.SetActive(false);
        }

        _rectTrans.DOAnchorPosY(650f, 0.5f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.Instance || !PhoneManager.Instance) { return; }
        float hp = PlayerManager.Instance.GetCurHPPercent();
        _canGrp.alpha = hp < 0.4f && !PhoneManager.Instance.IsActive() ? 1 : 0;
    }
}
