using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;

    Canvas canvas;
    [SerializeReference] TextMeshProUGUI ui;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void LoadScene(int buildIndex, bool withLoadingScreen)
    {
        AudioManager.instance.StopAllNonMusicLoops();
        StartCoroutine(LoadAsyncScene(buildIndex, withLoadingScreen));
    }

    IEnumerator LoadAsyncScene(int buildIndex, bool withLoadingScreen)
    {
        yield return null;

        if (withLoadingScreen)
        {
            canvas.enabled = true;
            ui.text = "0%";
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(buildIndex);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (withLoadingScreen)
                ui.text = (int)(asyncOperation.progress * 100) + "%";

            if (asyncOperation.progress >= .9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (withLoadingScreen)
            canvas.enabled = false;

        yield return null;
    }
}
