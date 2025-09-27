using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    public Material BarMat;
    private int percentId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        percentId = Shader.PropertyToID("_Percent");
        BarMat.SetFloat(percentId, 1);
    }

    public void SetHP(float hp)
    {
        BarMat.SetFloat(percentId, hp);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
