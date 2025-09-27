using Unity.Mathematics.Geometry;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float DrainRate = 1;
    public float MaxFatigue = 100;
    private float curFatigue = 100;
    public EnergyBar Bar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnergy(- DrainRate * Time.deltaTime);
    }

    private void UpdateEnergy(float change)
    {
        curFatigue += change;
        curFatigue = Mathf.Clamp(curFatigue, 0, MaxFatigue);
        float hp = curFatigue / MaxFatigue;
        Bar.SetHP(hp);
    }
}
