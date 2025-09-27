using UnityEngine;

public class WordManager : MonoBehaviour
{
    public static WordManager Get;
    [SerializeField] private WordBubble _wordBubblePrefab;
    [SerializeField] private float _bubbleDuration;
    [SerializeField] private float _bubbleRate;
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
        bubbleFab.Init(GetRandomWord(), _bubbleDuration);
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

    private string GetRandomWord()
    {
        return "asdf";
    }
} 
