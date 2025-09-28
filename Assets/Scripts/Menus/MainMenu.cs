using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string creditsScene;

    public void OnStartButtonClicked()
    {
        LoadingScreen.Instance.LoadNextRound(1);
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
