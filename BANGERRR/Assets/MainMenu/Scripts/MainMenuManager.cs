using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class MainMenuManager : MonoBehaviour
{
    public Scene SolarySystemScene;
    public GameObject MainMenuCanvas;
    public GameObject mainCamera;
    public int step = 0; //0 menu, 1-> chapter selection

    public GameObject QuitterButton;

    // Start is called before the first frame update
    void Awake()
    {
        ChapterManager.GetSave();
        Debug.Log("MAINMENU : " + ChapterManager.maxChapterIndexDiscoveredByPlayer);
    }

    private void Start()
    {
        StartCoroutine(nameof(MakeMainCameraWork));
        AudioManager.instance.FadeIn("theme", 240);
    }

    private IEnumerator MakeMainCameraWork()
    {
        yield return new WaitForSeconds(.1f);
        mainCamera.SetActive(false);
        yield return new WaitForSeconds(.1f);
        mainCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (step == 1)
        {
            //GetComponentInChildren<BackgroundLineManager>().setActiveFirstLine();
        }
        */

        if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            AudioManager.instance.Play("debug");
            ChapterManager.ResetProgression();
        }
    }

    public void displayChapterSelectionOrLaunchGame()
    {
        Debug.Log(ChapterManager.currentChapterIndex);

        if (ChapterManager.maxChapterIndexDiscoveredByPlayer != 0) //ALREADY PLAYED
        {
            GameObject.Find("Game_Logo").SetActive(false);
            GameObject.Find("Start_Btn").SetActive(false);
            QuitterButton.SetActive(false);

            mainCamera.GetComponent<CameraMover>().canMove = true;
        }
        else //NEW GAME
        {
            GlobalVariables.Set("planetIndex", 0);
            LoadSceneManager.instance.LoadScene(1, true);
        }
    }
}
