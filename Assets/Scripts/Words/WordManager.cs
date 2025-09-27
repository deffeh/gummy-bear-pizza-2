using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public static WordManager Get;
    [SerializeField] private WordBubble _wordBubblePrefab;
    [SerializeField] private float _bubbleDuration;
    [SerializeField] private float _bubbleRate;
    [SerializeField] private int _rewardWordsPerBubble;
    [SerializeField] private List<TextAsset> _textFiles;
    private List<List<string>> _listOfListsOfWords;
    private float _bubbleMultiplier;
    private float _wordLength;
    private float _baseBubbleRate;

    private void Awake()
    {
        if (Get == null)
        {
            Get = this;
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
    }


    private void Update()
    {
        _bubbleRate -= Time.deltaTime;
        if (_bubbleRate <= 0)
        {
            _bubbleRate = _baseBubbleRate;
            InstantiateBubble();
        }
    }

    public void InstantiateBubble()
    {
        WordBubble bubbleFab = Instantiate(_wordBubblePrefab, transform);
        bubbleFab.Init(GetRandomWordOfLength(Random.Range(3, 7)), _bubbleDuration, _rewardWordsPerBubble);
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
                result.Add(word);
            }
            _listOfListsOfWords.Add(result);
        }
    }
} 
