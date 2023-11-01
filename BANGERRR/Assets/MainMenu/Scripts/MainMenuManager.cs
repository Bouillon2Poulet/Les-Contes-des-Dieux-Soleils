using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    public Scene SolarySystemScene;
    public GameObject MainMenuCanvas;
    public GameObject mainCamera;
    public int step = 0; //0 menu, 1-> chapter selection

    // Start is called before the first frame update
    void Awake()
    {
        ChapterManager.getSaveFileJSONData();
        Debug.Log("MAINMENU : " + ChapterManager.maxChapterIndexDiscoveredByPlayer);
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void displayChapterSelectionOrLaunchGame()
    {
        Debug.Log(ChapterManager.currentChapterIndex);

        if (ChapterManager.maxChapterIndexDiscoveredByPlayer != 0) //ALREADY PLAYED
        {
            GameObject.Find("Game_Logo").SetActive(false);
            GameObject.Find("Start_Btn").SetActive(false);

            mainCamera.GetComponent<CameraMover>().canMove = true;
        }
        else //NEW GAME
        {
            SceneManager.SetActiveScene(SolarySystemScene);
        }
    }
}
