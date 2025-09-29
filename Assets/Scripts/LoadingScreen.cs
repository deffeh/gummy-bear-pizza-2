using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    public PetType petType;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _body;
    [SerializeField] private CanvasGroup _textCanvasGroup;
    [SerializeField] private CanvasGroup _canvasGrp;
    private bool _loading = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += Hide;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= Hide;
    }

    private void Hide(Scene arg0, LoadSceneMode arg1)
    {
        _loading = false;
        var seq = DOTween.Sequence();
        seq.Append(_canvasGrp.DOFade(0f, 1f));
        seq.Play();
    }

    public void LoadScene(string sceneName, string title, string body, float holdDuration)
    {
        if (_loading) { return; }
        _loading = true;
        _title.text = title;
        _body.text = body;
        _canvasGrp.alpha = 0f;
        _textCanvasGroup.alpha = 0f;
        var seq = DOTween.Sequence();
        seq.Append(_canvasGrp.DOFade(1f, 1.5f));
        if (holdDuration > 0 && (title != "" || body != ""))
        {
            seq.Append(_textCanvasGroup.DOFade(1f, 1.5f));
            seq.AppendInterval(holdDuration);
        }
        seq.AppendCallback(() =>
        {
            SceneManager.LoadSceneAsync(sceneName);
        });
        seq.Play();
    }

    public void LoadNextRound(int round)
    {
        LoadScene("GameScene", "Week " + round, GetRandomBodyText(), 3f);
    }
    private List<string> fuckBitches = new List<string>()
    {
 "You played Bald Guys 3 all week, died to the barber, ended your for haironour run.",
    "You spilled orange juice on your laptop and had to miss your capstone presentation.",
"BilkBong released and you skipped all of your classes. Did you even play the first one?",
    "You were too busy playing [NAME OF OUR GAME] and forgot to charge your laptop.",
    "You spent all week playing game jam submissions.",
    "You ate too many caniacs and had too many bobas and took a fat 5 day nap.",
    "You watched too many speedruns of OOT (copyright) and now you Stale Reference Manipulation'd your brain.",
    "You played 'Cutting it Reel Close' and was so inspired that you didn't want to do your paper.",
    "You tried playing the new [INSERT UE5 GAME] and compiling shaders took you 5 days straight and your computer caught on fire. Tough.",
    "You tried convincing your friends to create an online real-time DND for a 48 hour game jam, and you researched and made a presentation about Unity Online Services that took all week, and you end up not even making an online multiplayer game."
    };
    private string GetRandomBodyText()
    {
        return fuckBitches[UnityEngine.Random.Range(0, fuckBitches.Count)];
    }
}
