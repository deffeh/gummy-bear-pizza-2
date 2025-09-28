using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _body;
    [SerializeField] private CanvasGroup _textCanvasGroup;
    [SerializeField] private CanvasGroup _canvasGrp;
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
        Application.targetFrameRate = 60;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= Hide;
    }

    private void Hide(Scene arg0, LoadSceneMode arg1)
    {
        var seq = DOTween.Sequence();
        seq.Append(_canvasGrp.DOFade(0f, 1f));
        seq.Play();
    }

    public void LoadScene(string sceneName, string title, string body, float holdDuration)
    {
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
    
    private string GetRandomBodyText()
    {
        return "stoopid";
    }
}
