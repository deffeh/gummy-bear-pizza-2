using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _roundText;
    [SerializeField] private TMP_Text _wordCount;
    [SerializeField] private RectTransform _finishedText;
    [SerializeField] private float _gameTimer = 180f;
    [SerializeField] private float _wordCountLerpSpeed;
    private int _round = 0;
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

        DontDestroyOnLoad(gameObject);
        _baseTime = _gameTimer;
        _roundText.text = $"Round: {_round + 1}";
    }

    private void Start()
    {
        InitializeRound();
    }

    private void Update()
    {
        if (RoundOver) { return; }
        _gameTimer -= Time.deltaTime;
        UpdateTimeUI();
        if (_gameTimer <= 0)
        {
            EndGame(false);
        }

        int totalWC = DocumentPage.Instance.totalWordCount;
        if (lerpWC != totalWC)
        {
            lerpWC = Mathf.MoveTowards(lerpWC, totalWC, Time.deltaTime * _wordCountLerpSpeed);
            _wordCount.text = $"WC: {(int)lerpWC}";
        }
    }

    public void EndGame(bool win)
    {
        //stop everything
        RoundOver = true;
        WordManager.Instance.gameObject.SetActive(false); //stop bubble instantiations

        if (win)
        {
            _wordCount.text = $"WC: {WordsToWin}";
            _finishedText.localScale = Vector2.zero;
            _finishedText.gameObject.SetActive(true);
            var seq = DOTween.Sequence();
            seq.Append(_finishedText.DOScale(1.5f, 0.3f));
            seq.Append(_finishedText.DOScale(1f, 1f));
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                _finishedText.gameObject.SetActive(false);
                //go to result screen
                LoadingScreen.Instance.LoadScene("ResultScreen", "", "", 0);
            });
            seq.Play();
        }
        else
        {
            //go to main menu?
            _round = 0;
            LoadingScreen.Instance.LoadScene("MainMenu", "", "", 0);
        }
    }

    public void InitializeRound()
    {
        _round++;
        WordManager.Instance?.Init(); //stop bubble instantiations, pass in rounds to handle difficulty scaling
        //start intro cutscene?
        _gameTimer = _baseTime;
        RoundOver = false;
    }

    private void UpdateTimeUI()
    {
        TimeSpan baseTime = new TimeSpan(hours: 9, minutes: 0, seconds: 0);
        TimeSpan timeSpent = TimeSpan.FromMinutes(Mathf.Clamp(_baseTime - _gameTimer, 0, _baseTime));
        TimeSpan totalTime = baseTime + timeSpent;
        _timeText.text = $"{totalTime.ToString(@"h\:mm")}";
    }

    public string GetTime()
    {
        return _timeText.text;
    }

}
