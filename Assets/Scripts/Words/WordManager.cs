using System.Collections.Generic;
using System.Linq;
using Phone;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance;
    [SerializeField] private WordBubble _wordBubblePrefab;
    [SerializeField] public float _bubbleDuration;
    [SerializeField] public float _bubbleRate;
    [SerializeField] public int _rewardWordsPerBubble;
    [SerializeField] private List<TextAsset> _textFiles;
    [SerializeField] public float _critChance = 0.05f;
    private List<List<string>> _listOfListsOfWords;
    private int _round;
    public float _baseBubbleRate;

    private int _rewardMultiplier = 1;
    private float _multiplierDuration = 0.0f;

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
        LoadAllWords();
        _baseBubbleRate = _bubbleRate;
    }

    public void Init(int round)
    {
        //based off round and stats, set bubble rate, duration, damage here?
        _round = round;
        _bubbleRate = Random.Range(0.25f, 0.75f);
    }


    private void Update()
    {
        if (PhoneManager.Instance == null || PhoneManager.Instance.IsActive() || GameManager.Instance == null || GameManager.Instance.RoundOver){ return; }

        _bubbleRate -= Time.deltaTime;
        if (_bubbleRate <= 0)
        {
            float ratio = PlayerManager.Instance.GetCurHPPercent();
            float rateMultiplier = 1f;
            if (ratio < 0.33f)
            {
                rateMultiplier = 1.6f;
            }
            else if (ratio < 0.66f)
            {
                rateMultiplier = 1.25f;
            }
            
            _bubbleRate = _baseBubbleRate * rateMultiplier;
            InstantiateBubble();
        }

        if (_multiplierDuration > 0)
        {
            _multiplierDuration -= Time.deltaTime;
            if (_multiplierDuration <= 0)
                ResetRewardMultiplier();
        }
    }

    public void InstantiateBubble()
    {
        float multiplier = GetDurationMultiplierForRound(_round);
        float tiredMultiplier = 1f;
        float playerTired = PlayerManager.Instance.GetCurHPPercent();
        float tiredRewardMulti = 1f;
        if (playerTired < 0.33f)
        {
            tiredMultiplier = 0.4f;
            tiredRewardMulti = 0.75f;
        }
        else if (playerTired < 0.66f)
        {
            tiredMultiplier = 0.7f;
            tiredRewardMulti = 0.9f;
        }
        var dur = Mathf.Max(_bubbleDuration * multiplier * tiredMultiplier, 1f);
        bool isNegative = Random.Range(0, 2) == 0;
        int slightRandomRewardWords = (isNegative ? -1 : 1) * Random.Range(0, _rewardWordsPerBubble / 8);
        
        WordBubble bubbleFab = Instantiate(_wordBubblePrefab, transform);
        bubbleFab.Init(GetRandomWordOfLength(GetWordLengthForRound(_round)), dur, Mathf.CeilToInt((_rewardWordsPerBubble + slightRandomRewardWords) * tiredRewardMulti * _rewardMultiplier), _critChance);
        PlaceBubbleRandomly(bubbleFab);
    }

    private void PlaceBubbleRandomly(WordBubble bubbleFab)
    {
        //randomly place bubble
        Vector2 containerSize = GetComponent<RectTransform>().rect.size;
        Vector2 bubbleSize = bubbleFab.GetComponent<RectTransform>().rect.size;
        float xPadding = containerSize.x / 15f;
        float yPadding = containerSize.y / 12f;
        float xMin = -(containerSize.x / 2f) + (bubbleSize.x / 2f) + xPadding;
        float xMax = (containerSize.x / 2f) - (bubbleSize.x / 2f) - xPadding;
        float yMin = -(containerSize.y / 2f) + (bubbleSize.y / 2f) + yPadding;
        float yMax = (containerSize.y / 2f) - (bubbleSize.y / 2f) - yPadding;

        float randomX = Random.Range(xMin, xMax);
        if (randomX < -containerSize.x / 7f) { yMin += containerSize.y / 5f; }
        float randomY = Random.Range(yMin, yMax);
        bubbleFab.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector2(randomX, randomY), Quaternion.identity);
    }

    private int GetWordLengthForRound(int round)
    {
        float random = Random.Range(0f, 1f);
        switch (round)
        {
            case 1:
                if (random > 0.4f) { return 3; }
                else { return 4; }
            case 2:
                if (random > 0.6) { return 4; }
                else if (random > 0.3) { return 3; }
                else { return 5; }
            case 3:
                if (random > 0.6) { return 4; }
                else if (random > 0.3) { return 5; }
                else { return 6; }
            default:
                if (random > 0.4) { return 6; }
                else if (random > 0.15) { return 5; }
                else {return Random.Range(3, 5); }
        }
    }

    private float GetDurationMultiplierForRound(int round)
    {
        switch (round)
        {
            case 1:
                return 1f;
            case 2:
                return 0.85f;
            case 3:
                return 0.6f;
            case 4:
                return 0.4f;
            default:
                return 0.4f * Mathf.Pow(0.8f, round - 4);
        }
    }    

    private string GetRandomWordOfLength(int wordLength)
    {
        //zero indexed so minus 3 from wordLength
        if (_listOfListsOfWords.Count <= 0) { return ""; }

        int index = wordLength - 3;
        List<string> words = _listOfListsOfWords[index];
        if (words.Count <= 0) { return ""; }

        int randomIdx = Random.Range(0, words.Count);
        return words[randomIdx];
    }

    private void LoadAllWords()
    {
        _listOfListsOfWords = new List<List<string>>();
        foreach (TextAsset text in _textFiles)
        {
            List<string> result = new List<string>();
            var words = text.text.Split("\n");
            foreach (string word in words)
            {
                if (word.Length < 3 || word == null) { continue; }
                word.Trim();
                result.Add(word);
            }
            _listOfListsOfWords.Add(result);
        }
    }

    public void GainRewardMultiplier()
    {
        _rewardMultiplier++; // change this to read from upgrades if we have an upgrade that modifies the multiplier
        _multiplierDuration = 15.0f; // change this to read from upgrades if we have an upgrade that modifies the duration
        CritMeter.SetMultMeter(_rewardMultiplier, _multiplierDuration);
    }

    public void ResetRewardMultiplier()
    {
        _rewardMultiplier = 1;
        CritMeter.ResetMultMeter();
    }

    // For UI
    public float GetMultiplierDuration()
    {
        return _multiplierDuration;
    }

} 
