using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DocumentPage : MonoBehaviour
{
    public static DocumentPage Instance;
    public TextMeshProUGUI  text;
    public int wordLimit = 300;
    public string essay;

    public float lerpSpeed = 5f;
    
    private string[] essayArr;

    private StringBuilder curEssay;
    private StringBuilder goalEssay;
    
    private float curLerp = 0;
    private float goalLerp = 0;

    private int curWordCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        curEssay = new StringBuilder();
        goalEssay = new StringBuilder();
        essay = essay.TrimEnd();
        essayArr = essay.Split(" ");
        string test = "asdf";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }

        if (curLerp != goalLerp && goalLerp != 0)
        {
            curLerp = Mathf.Lerp(curLerp, goalLerp, lerpSpeed * Time.deltaTime);
            bool changed = false;
            while (curEssay.Length < curLerp)
            {
                changed = true;
                curEssay.Append(goalEssay[curEssay.Length]);
            }

            if (changed)
                text.text = curEssay.ToString();
        }
    }

    public void AddWords(int words)
    {
        for (int i = 0; i < words; i++)
        {
            goalEssay.Append(essayArr[curWordCount]);
            goalEssay.Append(" ");
            curWordCount++;
            goalLerp = goalEssay.Length;
        }

    }
}
