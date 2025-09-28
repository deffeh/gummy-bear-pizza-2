using TMPro;
using UnityEngine;

public class CopyTextx : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI thisText;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        thisText.text = text.text;
    }
}
