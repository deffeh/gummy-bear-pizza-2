using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    public string mainMenuScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGoBackClicked()
    {
        print("Moving to main");
        if (mainMenuScene.Length > 0)
        {
            SceneManager.LoadScene(mainMenuScene);
        }
        else
        {
            print("No scene dummy");
        }
    }
}
