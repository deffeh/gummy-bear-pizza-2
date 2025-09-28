using Phone;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.AI;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    public string creditsScene;

    public GameObject petMenu;

    public AudioSource bg;

    private bool fadingOut = false;

    private bool changeScene = false;

    private int whichScene;

    public void Start()
    {
        DestroyAllGameSingletons();
        Reel.timeLostPerReel = 3;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (fadingOut)
        {
            bg.volume -= 0.5f * Time.deltaTime;
            if (bg.volume <= 0.0f)
            {
                changeScene = true;
                if (whichScene == 0)
                {
                    startGame();
                }
                else
                {
                    goCredits();
                }
            }
        }
    }

    public void OnStartButtonClicked()
    {
        fadingOut = true;
        whichScene = 0;
    }

    private void startGame()
    {
        LoadingScreen.Instance.LoadNextRound(1);
    }

    public void OnCreditsButtonClicked()
    {
        fadingOut = true;
        whichScene = 1;
    }

    private void goCredits()
    {
        if (creditsScene.Length > 0)
        {
            SceneManager.LoadScene(creditsScene);
        }
    }

    public void OnPetsCustomButtonClicked()
    {
        petMenu.SetActive(true);
    }

    public void OnSelectPetButtonClicked(string petName)
    {

        PetType petType = (PetType)Enum.Parse(typeof(PetType), petName);
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
