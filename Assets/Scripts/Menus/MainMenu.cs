using Phone;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string creditsScene;

    public void Start()
    {
        DestroyAllGameSingletons();
        Reel.timeLostPerReel = 3;
        Application.targetFrameRate = 60;
    }

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

    private void DestroyAllGameSingletons()
    {
        //to reset the game state
        if (WordManager.Instance != null) { Destroy(WordManager.Instance.gameObject); }
        if (PlayerManager.Instance != null) { Destroy(PlayerManager.Instance.gameObject); }
    }
}
