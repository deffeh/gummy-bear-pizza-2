using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Math = Unity.Mathematics.Geometry.Math;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TMP_Text _wordCount;
    [SerializeField] private RectTransform _finishedText;
    [SerializeField] private RectTransform _timesUpText;
    
    [SerializeField] private TMP_Text _stickyNoteText;
    [SerializeField] public float _gameTimer = 180f;
    [SerializeField] private float _wordCountLerpSpeed;
    [SerializeField] private CanvasGroup _firstONe;
    [SerializeField] private CanvasGroup _secondONe;
    [SerializeField] private CanvasGroup _thirdONe;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _gong;
    [SerializeField] private AudioClip _timpani;
    [SerializeField] private AudioClip _finished;


    private float _baseTime;
    public int WordsToWin = 1000;
    public bool RoundOver = true;
    private float lerpWC = 0;

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

        _baseTime = _gameTimer;
    }

    private void Start()
    {
        InitializeRound();
    }

    private void Update()
    {
        if (RoundOver) { return; }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame(true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            EndGame(false);
            return;
        }
#endif

            _gameTimer -= Time.deltaTime;
        if (_gameTimer <= 0)
        {
            EndGame(false);
        }

        int totalWC = DocumentPage.Instance.totalWordCount;
        if (lerpWC != totalWC)
        {
            lerpWC = Mathf.Lerp(lerpWC, totalWC, Time.deltaTime * _wordCountLerpSpeed);
            SetWC(lerpWC);
        }
    }

    public void EndGame(bool win)
    {
        if (RoundOver) { return; }
        //stop everything
        RoundOver = true;

        if (win)
        {
            SetWC(WordsToWin);
            _finishedText.localScale = Vector2.zero;
            _finishedText.gameObject.SetActive(true);
            PlayerManager.Instance.maxTime = TimeSpan.FromMinutes(_baseTime);
            PlayerManager.Instance.remainingTime = TimeSpan.FromMinutes(_gameTimer);
            _audioSource.PlayOneShot(_finished);
            var seq = DOTween.Sequence();
            seq.Append(_finishedText.DOScale(1.5f, 0.3f));
            seq.Append(_finishedText.DOScale(1f, 1f));
            seq.AppendInterval(2f);
            seq.AppendCallback(() =>
            {
                //go to result screen
                LoadingScreen.Instance.LoadScene("UpgradeScene", "", "", 0);
            });
            seq.Play();
        }
        else
        {
            _timesUpText.localScale = Vector2.zero;
            _timesUpText.gameObject.SetActive(true);
            var seq = DOTween.Sequence().SetEase(Ease.OutQuad);
            seq.Append(_timesUpText.DOScale(1.5f, 2.5f));
            seq.AppendInterval(1.5f);
            seq.AppendCallback(() =>
            {
                LoadingScreen.Instance.LoadScene("MainMenuScene", "FAILURE", "Shouldn't have used all your slipdays...", 2f);
            });
            seq.Play();
        }
    }

    public void InitializeRound()
    {
        SetWC(0);
        RoundOver = true;
        int round = PlayerManager.Instance.round;
        WordsToWin = Mathf.CeilToInt(WordsToWin * Mathf.Pow(1.75f, round - 1));
        if (round == 1)
        {
            WordsToWin = 300;
        }
        _stickyNoteText.text = $"{WordsToWin} WORDS DUE MIDNIGHT";
        PlayerManager.Instance.InstantSetHPToMax();
        PlayerManager.Instance.SetBarActive(true);

        _firstONe.GetComponent<RectTransform>().localScale = Vector2.zero;
        _secondONe.GetComponent<RectTransform>().localScale = Vector2.zero;
        _secondONe.GetComponent<TMP_Text>().text = $"{WordsToWin} WORDS DUE AT MIDNIGHT";  
        _thirdONe.GetComponent<RectTransform>().localScale = Vector2.zero;

        var seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.Append(_firstONe.GetComponent<RectTransform>().DOScale(8f, 2.5f));
        seq.JoinCallback(() => _audioSource.PlayOneShot(_timpani));
        seq.Join(_firstONe.DOFade(0f, 1f).SetDelay(1.5f));
        seq.Append(_secondONe.GetComponent<RectTransform>().DOScale(4f, 2.5f));
        seq.JoinCallback(() => _audioSource.PlayOneShot(_timpani));
        seq.Join(_secondONe.DOFade(0f, 1f).SetDelay(1.5f));
        seq.Append(_thirdONe.GetComponent<RectTransform>().DOScale(1f, 1f));
        seq.JoinCallback(() => _audioSource.PlayOneShot(_gong));
        seq.Join(_thirdONe.DOFade(1f, 0.5f));
        seq.OnComplete(() =>
        {
            WordManager.Instance?.Init(PlayerManager.Instance.round);
            _gameTimer = _baseTime;
            RoundOver = false;
            _thirdONe.DOFade(0f, 2f);
        });
        seq.Play();
    }

    public string GetTime()
    {
        TimeSpan baseTime = new TimeSpan(hours: 9, minutes: 0, seconds: 0);
        TimeSpan timeSpent = TimeSpan.FromMinutes(Mathf.Clamp(_baseTime - _gameTimer, 0, _baseTime));
        TimeSpan totalTime = baseTime + timeSpent;
        return $"{totalTime.ToString(@"h\:mm")}";
    }

    private void SetWC(float wc)
    {
        _wordCount.text = $"<b>{(int)Mathf.Ceil(wc)}</b> words";
    }

}
