using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DocumentPage : MonoBehaviour
{
    public static DocumentPage Instance;
    public Page PagePrefab;
    public bool debug = false;
    private Page curPage;
    private TextMeshProUGUI text;
    public int wordLimit = 300;
    public string essay;

    public float lerpSpeed = 5f;
    
    private string[] essayArr;

    private StringBuilder curEssay;
    private StringBuilder goalEssay;
    
    private float curLerp = 0;
    private float goalLerp = 0;

    private int curWordCount = 0;
    private int pageWordCount = 0;
    public Vector2 PageTossForce = new Vector2(100, 100);
    public float RotationForce = 100;
    public int totalWordCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        essay = essay.TrimEnd();
        essayArr = essay.Split(" ");
        NewPage();
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddWords(100);
            } 
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

    private void NewPage()
    {
        if (curPage)
        {
            curPage.rb.simulated = true;
            float x = Random.Range(-PageTossForce.x, PageTossForce.x);
            float y = Random.Range(PageTossForce.y * .4f, PageTossForce.y);
            curPage.rb.AddForce(new Vector2(x, y));
            curPage.rb.AddTorque(Random.Range(-RotationForce, RotationForce));
            curPage.text.text = goalEssay.ToString();
            Destroy(curPage.gameObject, 10f);
        }
        pageWordCount = 0;
        curEssay = new StringBuilder();
        goalEssay = new StringBuilder();
        curLerp = 0;
        curPage = Instantiate(PagePrefab, transform);
        text = curPage.text;
        curPage.transform.SetAsFirstSibling();
    }
    public void AddWords(int words)
    {
        for (int i = 0; i < words; i++)
        {
            goalEssay.Append(essayArr[curWordCount]);
            goalEssay.Append(" ");
            totalWordCount++;
            curWordCount++;
            pageWordCount++;
            if (curWordCount >= essayArr.Length) curWordCount = 0;
            if (pageWordCount > wordLimit)
            {
                NewPage();
            }
            goalLerp = goalEssay.Length;
        }

        if (totalWordCount >= GameManager.Instance.WordsToWin)
        {
            GameManager.Instance.EndGame(true);
        }
        
    }
}
