using TMPro;
using UnityEngine;

public class WordBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _lifetime;
    [SerializeField] private string _word;
    private int _curWordIndex;
    private bool _completed;

    public void Start()
    {
        ResetWord();
    }

    public void Init(string word, float lifetime)
    {
        _word = word;
        _lifetime = lifetime;
        _curWordIndex = 0;
        ResetWord();
    }

    public void Update()
    {
        if (_lifetime <= 0) { return; }

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
}
