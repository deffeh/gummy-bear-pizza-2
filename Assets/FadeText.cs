using TMPro;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float duration = 2;
    private float curDuration = 0;
    public AnimationCurve curve;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, curve.Evaluate(curDuration / duration));
        curDuration -= Time.deltaTime;
    }
}
