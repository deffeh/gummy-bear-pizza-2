using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _roundText;
    [SerializeField] private TMP_Text _wordCount;
    [SerializeField] private float _gameTimer = 180f;
    [SerializeField] private float _wordCountLerpSpeed;
    private bool _tick = true;
    private int _round;
    private float _baseTime;
    public int WordsToWin = 1000;
    public bool RoundOver = false;
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
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (RoundOver) { return; }

        if (_tick)
        {
            //time
            _gameTimer -= Time.deltaTime;
            UpdateTimeUI();
            if (_gameTimer <= 0)
            {
                _tick = false;
                EndGame(false);
            }
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
            //show result screen and upgrade screen?
        }
        else
        {
            //go to main menu?
        }
    }

    private void StartNextRound()
    {
        RoundOver = false;
        _round++;
        WordManager.Instance.Init();
        _gameTimer = _baseTime;
        _tick = true;
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
