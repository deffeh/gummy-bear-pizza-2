using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Gradient gradient;
    private Material BarMat;
    [SerializeField] private float _lerpSpeed = 5f;
    public Image BarImage;
    private int percentId;
    private float _lerpVal = 1;
    private float _targetVal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        BarMat = new Material(BarImage.material);
        BarImage.material = BarMat;
        percentId = Shader.PropertyToID("_Percent");
        BarMat.SetFloat(percentId, 1);
    }

    public void InstantSetHP(float hp)
    {
        _lerpVal = hp;
        _targetVal = hp;
        BarMat.SetFloat(percentId, hp);
        BarImage.color = gradient.Evaluate(_targetVal);
    }

    public void SetHP(float hp)
    {
        _targetVal = hp;
    }
    // Update is called once per frame
    void Update()
    {
        if (_lerpVal != _targetVal)
        {
            _lerpVal = Mathf.Lerp(_lerpVal, _targetVal, Time.deltaTime * _lerpSpeed);
            BarMat.SetFloat(percentId, _lerpVal);
            BarImage.color = gradient.Evaluate(_lerpVal);
        }   
    }
}
