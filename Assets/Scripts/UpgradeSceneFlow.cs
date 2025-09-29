using System;
using DG.Tweening;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class UpgradeSceneFlow : MonoBehaviour
{
    [SerializeField] private RectTransform _pageOne;
    [SerializeField] private RectTransform _pageTwo;
    [SerializeField] private GameObject _panelOne;
    [SerializeField] private GameObject _timeRemainTitle;
    [SerializeField] private TMP_Text _timeRemain;
    [SerializeField] private GameObject _goldObtainedTitle;
    [SerializeField] private TMP_Text _goldObtained;
    [SerializeField] private TMP_Text _RANK;
    private int lerpVal;
    [SerializeField] private float lerpDuration = 5f;
    [SerializeField] private AudioSource _audioSource;

    public void Awake()
    {
        if (PlayerManager.Instance) { PlayerManager.Instance.SetBarActive(false); }
        //calculate remaining time and rank
        TimeSpan remainingTime = PlayerManager.Instance.remainingTime;
        TimeSpan maxTime = PlayerManager.Instance.maxTime;
        string rank = GetRank(remainingTime, maxTime);
        int coins = GetCoinsFromRank(rank);
        PlayerManager.Instance.totalMoney += coins;
        _RANK.text = rank;

        var seq = DOTween.Sequence();
        seq.AppendCallback(() => _panelOne.SetActive(true));
        seq.AppendInterval(2f);
        seq.AppendCallback(() => { _timeRemainTitle.SetActive(true); _panelOne.SetActive(false); _audioSource.Play(); });
        float stepDur = 1.5f;
        seq.AppendInterval(stepDur);
        seq.AppendCallback(() =>
        {
            if (remainingTime.Hours == 0 && remainingTime.Minutes == 0)
            {
                _timeRemain.text = "Less than a minute!!!";
            }
            else
            {
                _timeRemain.text = $"{remainingTime.Hours} hours and {remainingTime.Minutes} minutes";
            }
            _timeRemain.gameObject.SetActive(true);
            _audioSource.Play();
        });
        seq.AppendInterval(stepDur);
        seq.AppendCallback(() => { _goldObtainedTitle.SetActive(true); _audioSource.Play();});
        seq.AppendInterval(stepDur);
        seq.AppendCallback(() => { _goldObtained.gameObject.SetActive(true); _audioSource.Play();});
        seq.AppendInterval(stepDur / 2f);
        seq.Append(DOTween.To(() => lerpVal, x =>
        {
            lerpVal = x;
            _goldObtained.text = lerpVal.ToString();
        }, coins, lerpDuration)).SetEase(Ease.InOutQuad);
        seq.AppendInterval(stepDur / 4f);
        seq.Append(_RANK.GetComponent<CanvasGroup>().DOFade(1f, 1f));
        seq.AppendInterval(2f);
        float width = _pageOne.sizeDelta.x;
        seq.Append(_pageOne.DOAnchorPos(new Vector2(-width, 0), 1f));
        seq.Join(_pageTwo.DOAnchorPos(Vector2.zero, 1f));
        seq.Play();
    }

    private string GetRank(TimeSpan timeTaken, TimeSpan maxTime)
    {
        double ratio = timeTaken.TotalMinutes / maxTime.TotalMinutes;
        if (ratio > 0.45f)
        {
            return "A";
        }
        else if (ratio > 0.3f)
        {
            return "B";
        }
        else if (ratio > 0.1f)
        {
            return "C";
        }
        else
        {
            return "D";
        }
    }

    private int GetCoinsFromRank(string rank)
    {
        switch (rank)
        {
            case "A":
                return 650;
            case "B":
                return 500;
            case "C":
                return 350;
            case "D":
                return 150;
            default:
                return 0;
        }
    }
}
