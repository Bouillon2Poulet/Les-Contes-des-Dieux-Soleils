using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ChapterSelectionManager : MonoBehaviour
{
    public static ChapterSelectionManager instance;

    public GameObject TritonPrefab;
    public GameObject EDPrefab;
    public GameObject SolisedePrefab;
    public GameObject SolimontPrefab;
    public GameObject AmphipolisPrefab;
    public GameObject OeilPrefab;
    public GameObject CentreUniversPrefab;

    public GameObject PlanetsCamera;
    public GameObject PlanetPrefabsEmpty;
    public GameObject UICanvas;

    public GameObject MiddleLine;
    public GameObject RightLine;
    public GameObject LeftLine;

    public float OffSet = 100f;
    public float rotationSpeed = 25;
    public float cameraSpeed = 75;

    private List<GameObject> PlanetsPrefabs;
    private List<GameObject> ChaptersTitles = new List<GameObject>();
    private uint currentPlanetIndex = 0;
    // public uint maxPlanetIndex = 3;

    public float TitleFadeSpeed;
    public Font font;
    public bool TitlesAreActive = false;
    private int cameraIsMoving = 0; //0 = no movement, 1 = right, -1 = left
    public GameObject titleBackground;

    public Image JouerBtnFromSelection;
    public Color hideColor;
    public Color showColor;

    public GameObject ArrowRight;
    public GameObject ArrowLeft;

    readonly string[] ChapterNames = { "Triton", "Eaux Divines", "Solisède", "Solimont", "Amphipolis", "Oeil", "Omnio" };
    readonly Dictionary<int, string> ChapterTranslation = new Dictionary<int, string>();

    // Start is called before the first frame update
    void Start()
    {
        InitTranslations();
        PlanetsPrefabs = new List<GameObject>{
            Instantiate(TritonPrefab),
            Instantiate(EDPrefab),
            Instantiate(SolisedePrefab),
            Instantiate(SolimontPrefab),
            Instantiate(AmphipolisPrefab),
            Instantiate(OeilPrefab),
            Instantiate(CentreUniversPrefab)
        };

        //ChaptersTitles = new List<GameObject>();

        float[] PrefabResizes = new float[] { 1.15f, 0.53f, 0.51f, 10.5f, 17.8f, 6.84f, 1.61f };

        for (int j = 0; j < PlanetsPrefabs.Count; j++)
        {
            float size = PrefabResizes[j];
            PlanetsPrefabs[j].transform.localScale = new Vector3(size, size, size);
        }

        int i = 0;
        foreach (GameObject planet in PlanetsPrefabs)
        {
            planet.transform.SetParent(PlanetPrefabsEmpty.transform);
            planet.name = "Planet " + i;
            planet.transform.position = new Vector3(i * OffSet, 0, 0);

            ChaptersTitles.Add(new GameObject());
            ChaptersTitles.Last().name = ((ChapterManager.Chapter)i).ToString() + "_title";
            ChaptersTitles.Last().AddComponent<Text>();
            ChaptersTitles.Last().GetComponent<Text>().text = "Chapitre " + i + " " + ChapterNames[i];
            ChaptersTitles.Last().GetComponent<Text>().font = font;
            ChaptersTitles.Last().GetComponent<Text>().fontSize = 60;
            ChaptersTitles.Last().GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            ChaptersTitles.Last().GetComponent<RectTransform>().sizeDelta = new Vector2(450, 180);
            ChaptersTitles.Last().transform.SetParent(UICanvas.transform);
            ChaptersTitles.Last().GetComponent<RectTransform>().localPosition = new Vector3((((i * OffSet)) * 100), 350, 0);
            ChaptersTitles.Last().SetActive(false);
            i++;
        }

        if (PlayerPrefs.HasKey("lang"))
        {
            LanguageManager.Lang lang = (LanguageManager.Lang)GlobalVariables.Get<int>("lang");
            UpdateTitlesLang(lang);
        }
    }

    public void UpdateTitlesLang(LanguageManager.Lang lang)
    {
        int i = 0;
        foreach (GameObject chapterTitle in ChaptersTitles)
        {
            chapterTitle.GetComponent<Text>().text = ChapterTranslation[(int)lang] + i + " " + ChapterNames[i++];
        }
        Debug.Log("Updated chapter titles to " + lang);
    }

    void InitTranslations()
    {
        ChapterTranslation[(int)LanguageManager.Lang.French] = "Chapitre ";
        ChapterTranslation[(int)LanguageManager.Lang.English] = "Chapter ";
    }

    void Update()
    {
        foreach (GameObject planet in PlanetsPrefabs)
        {
            planet.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * rotationSpeed);
        }
        if (GetComponent<MainMenuManager>().step == 1)
        {
            if (!TitlesAreActive)
            {
                foreach (GameObject title in ChaptersTitles)
                {
                    title.SetActive(true);
                    RightLine.SetActive(true);
                    LeftLine.SetActive(true);
                    TitlesAreActive = true;
                    ArrowRight.SetActive(true);
                    ArrowLeft.SetActive(true);
                }
            }
            if (cameraIsMoving == 0)
            {
                if (currentPlanetIndex == 0)
                {
                    ArrowLeft.SetActive(false);
                    ArrowRight.SetActive(true);
                }
                else if (currentPlanetIndex == ChapterManager.maxChapterIndexDiscoveredByPlayer)
                {
                    ArrowLeft.SetActive(true);
                    ArrowRight.SetActive(false);
                }
                else
                {
                    ArrowLeft.SetActive(true);
                    ArrowRight.SetActive(true);
                }
                JouerBtnFromSelection.color = showColor;
                //titleBackground.SetActive(true);
            }
            else
            {
                //GetComponentInChildren<BackgroundLineManager>().moveLines(PlanetsCamera.transform.position.x, OffSet, cameraIsMoving);

                PlanetsCamera.transform.Translate(new Vector3(cameraIsMoving * Time.deltaTime * cameraSpeed, 0, 0));

                if (cameraIsMoving == 1 && PlanetsCamera.transform.position.x > currentPlanetIndex * OffSet)
                {
                    PlanetsCamera.transform.position = new Vector3(currentPlanetIndex * OffSet, 0, -60);
                    cameraIsMoving = 0;
                    // if (currentPlanetIndex == (int)ChapterManager.saveObject.maxChapterDiscovered)
                    // {
                    //     RightLine.GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    // }
                    if (currentPlanetIndex == (int)0)
                    {
                        RightLine.GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    }
                    else
                    {
                        RightLine.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                    }
                }
                else if (cameraIsMoving == -1 && PlanetsCamera.transform.position.x < currentPlanetIndex * OffSet)
                {
                    PlanetsCamera.transform.position = new Vector3(currentPlanetIndex * OffSet, 0, -60);
                    cameraIsMoving = 0;
                }
                for (int i = 0; i < PlanetsPrefabs.Count; i++)
                {
                    Vector3 newPos = new Vector3();
                    newPos.x = (((i * OffSet) - PlanetsCamera.transform.position.x) * 100);
                    newPos.y = 350;
                    newPos.z = 0;
                    ChaptersTitles[i].GetComponent<RectTransform>().localPosition = newPos;
                    float colorValue = Mathf.Lerp(0, 100, 980 / 980 - Mathf.Abs(newPos.x / TitleFadeSpeed));
                    ChaptersTitles[i].GetComponent<UnityEngine.UI.Text>().color = new Color(colorValue, colorValue, colorValue, 1);
                }
            }
        }
    }

    public void StartFromSelection()
    {
        if (cameraIsMoving == 0 && GetComponent<MainMenuManager>().step == 1)
        {
            ChapterManager.currentChapterIndex = (int)currentPlanetIndex;
            //SceneManager.SetActiveScene(GetComponent<MainMenuManager>().SolarySystemScene);

            Debug.Log("Launching on chapter " + ChapterManager.currentChapterIndex);

            GlobalVariables.Set("planetIndex", ChapterManager.currentChapterIndex);
            if (ChapterManager.currentChapterIndex > 0)
            {
                AudioManager.instance.FadeOut("theme", 120);
            }
            if (ChapterManager.currentChapterIndex < 6)
            {
                LoadSceneManager.instance.LoadScene(1, true);
            }
            else
            {
                LoadSceneManager.instance.LoadScene(2, true);
            }
        }
    }

    public void moveRight()
    {
        if (cameraIsMoving == 0)
        {
            currentPlanetIndex++;
            cameraIsMoving = 1;
            //GetComponentInChildren<BackgroundLineManager>().createLine(cameraIsMoving, (int)currentPlanetIndex);
            JouerBtnFromSelection.color = hideColor;
        }
    }


    public void moveLeft()
    {
        if (cameraIsMoving == 0)
        {
            currentPlanetIndex--;
            cameraIsMoving = -1;
            //GetComponentInChildren<BackgroundLineManager>().createLine(cameraIsMoving, (int)currentPlanetIndex);
            JouerBtnFromSelection.color = hideColor;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}