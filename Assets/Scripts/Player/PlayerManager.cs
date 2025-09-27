using Unity.Mathematics.Geometry;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public float DrainRate = 1;
    public float MaxFatigue = 100;
    private float curFatigue = 100;
    public EnergyBar Bar;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnergy(- DrainRate * Time.deltaTime);
    }

    public void UpdateEnergy(float change)
    {
        curFatigue += change;
        curFatigue = Mathf.Clamp(curFatigue, 0, MaxFatigue);
        float hp = curFatigue / MaxFatigue;
        Bar.SetHP(hp);
    }
}
