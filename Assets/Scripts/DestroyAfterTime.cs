using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float Lifetime = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, Lifetime);
    }

}
