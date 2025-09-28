using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CritMeter : MonoBehaviour
{
    public static CritMeter Instance;
    public Image Meter;
    public Image Fire;
    public TextMeshProUGUI MultText;
    public AnimationCurve ScaleCurve;
    public AnimationCurve RotationCurve;
    public float AnimDuration = 1.4f;
    public float RotationAmount = 30;
    public float ScaleMult = 1.4f;
    public Gradient ColorGradient;
    private float startScale = 1;
    private float curDuration = 0;
    private float maxDuration = 1;
    public AnimationCurve FireFade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        Meter.fillAmount = 0;
        MultText.text = "";
        startScale = transform.localScale.x;
        Fire.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        if (curDuration > 0 && maxDuration != 0)
        {
            Meter.fillAmount = curDuration / maxDuration;
            curDuration -= Time.deltaTime;
            float f = curDuration / maxDuration;
            Meter.color = ColorGradient.Evaluate(f);
            MultText.color = ColorGradient.Evaluate(f);
            Fire.color = new Color(1, 1, 1, ColorGradient.Evaluate(f).a);
            if (curDuration <= 0)
            {
                ResetMeter();
            }
        }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     SetMeter(2, 5);
        // }
    }

    public void ResetMeter()
    {
        Meter.fillAmount = 0;
        MultText.text = "";
        curDuration = 0;
        Fire.color = Color.clear;
    }

    public static void SetMultMeter(float inMult, float inDuration)
    {
        if(!Instance) return;
        if(inMult <= 1 || inDuration <= 0)
        {
            Instance.ResetMeter();
            return;
        }
        Instance.SetMeter(inMult, inDuration);
    }
    public static void ResetMultMeter()
    {
        if(!Instance) return;
        Instance.ResetMeter();
    }
    public void SetMeter(float inMult, float inDuration)
    {
        if (inMult <= 1)
        {
            ResetMeter();
            return;
        }
        MultText.text = "x" + inMult;
        curDuration = inDuration;
        maxDuration = inDuration;
        StopAllCoroutines();
        StartCoroutine(MultAnim());
    }

    private IEnumerator MultAnim()
    {
        float duration = AnimDuration;
        while (duration > 0)
        {
            transform.localScale = Vector3.Lerp(Vector3.one * startScale , Vector3.one * ScaleMult * startScale, ScaleCurve.Evaluate(duration / AnimDuration));
            transform.rotation = Quaternion.Euler(0,0, RotationCurve.Evaluate(duration / AnimDuration) * RotationAmount);
            yield return null;
            duration -= Time.deltaTime;
        }
        
    }
}
