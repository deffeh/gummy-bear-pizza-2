using System.Collections.Generic;
using System.Linq;
using Phone;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance;
    [SerializeField] private WordBubble _wordBubblePrefab;
    [SerializeField] private float _bubbleDuration;
    [SerializeField] private float _bubbleRate;
    [SerializeField] private int _rewardWordsPerBubble;
    [SerializeField] private List<TextAsset> _textFiles;
    private List<List<string>> _listOfListsOfWords;
    private float _wordLength;
    private float _baseBubbleRate;

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
        Init();
    }

    public void Init()
    {
        _baseBubbleRate = _bubbleRate;
        gameObject.SetActive(true);
    }


    private void Update()
    {
        if (PhoneManager.Instance != null && PhoneManager.Instance.IsActive()){ return; }

        _bubbleRate -= Time.deltaTime;
        if (_bubbleRate <= 0)
        {
            _bubbleRate = _baseBubbleRate;
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
        WordBubble bubbleFab = Instantiate(_wordBubblePrefab, transform);
        bubbleFab.Init(GetRandomWordOfLength(Random.Range(3, 7)), _bubbleDuration, _rewardWordsPerBubble * _rewardMultiplier);
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
        float randomY = Random.Range(yMin, yMax);
        bubbleFab.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector2(randomX, randomY), Quaternion.identity);
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
        _rewardMultiplier *= 2; // change this to read from upgrades if we have an upgrade that modifies the multiplier
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
