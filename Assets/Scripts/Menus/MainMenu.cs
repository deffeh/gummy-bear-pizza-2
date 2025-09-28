using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string creditsScene;

    public void Start()
    {
        DestroyAllGameSingletons();
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
        if (GameManager.Instance != null) { Destroy(GameManager.Instance.gameObject); }
        if (WordManager.Instance != null) { Destroy(WordManager.Instance.gameObject); }
        if (PlayerManager.Instance != null) { Destroy(PlayerManager.Instance.gameObject); }
    }
}
