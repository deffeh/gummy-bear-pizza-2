using Phone;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public string creditsScene;

    public GameObject petMenu;
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

    public void OnPetsCustomButtonClicked()
    {
        print("Change ur pet");
        petMenu.SetActive(true);
    }

    public void OnSelectPetButtonClicked(string petName)
    {

        PetType petType = (PetType)Enum.Parse(typeof(PetType), petName);
        print("Setting to " + petType);
        GameObject loadingScreenObject = GameObject.Find("LoadingScreenCanvas");
        LoadingScreen loadingScreen = loadingScreenObject.GetComponent<LoadingScreen>();
        loadingScreen.petType = petType;
        petMenu.SetActive(false);
    }

    private void DestroyAllGameSingletons()
    {
        //to reset the game state
        if (WordManager.Instance != null) { Destroy(WordManager.Instance.gameObject); }
        if (PlayerManager.Instance != null) { Destroy(PlayerManager.Instance.gameObject); }
    }
}
