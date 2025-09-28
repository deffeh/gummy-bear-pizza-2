using TMPro;
using UnityEngine;

public class WordBubble : MonoBehaviour
{
    public RectTransform TextExplosion;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _lifetime = 1f;
    [SerializeField] private string _word;
    [SerializeField] private int _rewardAmount;
    [SerializeField] private float _critChance;
    [SerializeField] private float _fatigueAmount = 10f;

    private int _curWordIndex;
    private bool _completed;

    public void Start()
    {
        ResetWord();
    }

    public void Init(string word, float lifetime, int rewardAmount)
    {
        _word = word.Trim();
        if (word.Length > 3) { ResizeBubble(word.Length); }
        _lifetime = lifetime;
        _curWordIndex = 0;
        _rewardAmount = rewardAmount;
        ResetWord();
    }

    public void Update()
    {
        if (GameManager.Instance.RoundOver) { return; }

        //Get all keyboard input
        if (Input.anyKeyDown)
        {
            string input = Input.inputString;
            HandleInput(input);
        }

        //Handle lifetime
        if (_lifetime > 0 && !_completed)
        {
            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0)
            {
                //kill itself
                Destroy(gameObject);
            }
        }
    }

    private void HandleInput(string input)
    {
        if (input == null || input.Length == 0) { return; }
        int len = input.Length;
        if (_word.Substring(_curWordIndex, len) == input)
        {
            _curWordIndex += len;
            if (_curWordIndex >= _word.Length)
            {
                _text.text = WrapStringInColor(_text.text, "000000");
                OnComplete();
            }
            else
            {
                string typed = _word.Substring(0, _curWordIndex);
                string untyped = _word.Substring(_curWordIndex);
                _text.text = WrapStringInColor(typed, "000000") + WrapStringInColor(untyped, "00000080");
            }
        }
        else
        {
            ResetWord();
        }
    }

    private void OnComplete()
    {
        _completed = true;
        float attemptCrit = Random.Range(0f, 1f);
        
        DocumentPage.Instance?.AddWords(attemptCrit < _critChance ? _rewardAmount * 2 : _rewardAmount);
        PlayerManager.Instance?.UpdateEnergy(-_fatigueAmount);
        RectTransform explosion = Instantiate(TextExplosion, transform.parent);
        explosion.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        Destroy(gameObject);
    }

    private void ResetWord()
    {
        _curWordIndex = 0;
        _text.text = WrapStringInColor(_word, "00000080");
    }

    private string WrapStringInColor(string str, string colorHex)
    {
        return $"<color=#{colorHex}>{str}</color>";
    }

    private void ResizeBubble(int wordLen)
    {
        float width = GetComponent<RectTransform>().rect.width;
        float newWidth = width *= (1 + (.2f * (wordLen - 3)));
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }
    
}
