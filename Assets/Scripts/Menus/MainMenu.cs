using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string gameScene;
    public string creditsScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartButtonClicked()
    {
        print("Moving to gamer");
        if (gameScene.Length > 0)
        {
            SceneManager.LoadScene(gameScene);
        }
        else
        {
            print("No scene dummy");
        }
    }

    public void OnCreditsButtonClicked()
    {
        print("Go to da credits");
        if (creditsScene.Length > 0)
        {
            SceneManager.LoadScene(creditsScene);
        }
        else
        {
            print("No scene dummy");
        }
    }
}
