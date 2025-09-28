using UnityEngine;

public class AddForceOnSpawn : MonoBehaviour
{

    public Rigidbody2D rb;
    public Vector2 TossForce = new Vector2(120, 40);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float x = Random.Range(-TossForce.x, TossForce.x);
        float y = Random.Range(TossForce.y * .4f, TossForce.y);
        rb.AddForce(new Vector2(x, y));
        rb.AddTorque(Random.Range(-30, 30));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
