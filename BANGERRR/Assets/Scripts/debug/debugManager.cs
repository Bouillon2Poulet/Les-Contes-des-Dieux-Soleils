using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class debugManager : MonoBehaviour
{
    public TextMeshProUGUI textOnCanvas;
    public Rigidbody player;
    public ThirdPersonMovement movements;
    [Header("TPs")]
    [SerializeField] public Transform[] Triton;
    [SerializeField] public Transform[] EauxDivines;
    [SerializeField] public Transform[] Solisede;
    [SerializeField] public Transform[] Solimont;
    [SerializeField] public Transform[] Amphipolis;
    [SerializeField] public Transform[] Larme;
    [SerializeField] public Transform[] Oeil;

    public int currentPage = 0;

    private int totalPage = 0;
    private List<Transform[]> pages;
    private KeyCode[] inputs;
    private string[] pageNames;
    private bool isShown = false;

    public GameObject arpenteur;

    private void Start()
    {
        if (PlayerPrefs.HasKey("lang"))
        {
            GlobalVariables.Set("lang", PlayerPrefs.GetInt("lang"));
        }

        pageNames = new string[] { "Triton", "Eaux Divines", "Solisède", "Solimont", "Amphipolis", "Larme", "Oeil" };
        pages = new List<Transform[]>
        {
            Triton,
            EauxDivines,
            Solisede,
            Solimont,
            Amphipolis,
            Larme,
            Oeil
        };

        totalPage = pages.Count;
        UpdateUI();

        inputs = new KeyCode[] { KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9 };

        /// Exemple de variable globale
        // GlobalVariables.Set("test", 1);
        // Debug.Log(GlobalVariables.Get<int>("test"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            AudioManager.instance.Play("debug");
            if (arpenteur.activeSelf)
            {
                arpenteur.SetActive(false);
                arpenteur.GetComponent<Arpenteur>().cam.Priority = 0;
                FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            }
            else
            {
                arpenteur.SetActive(true);
                arpenteur.GetComponent<Arpenteur>().cam.Priority = 1000;
                FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
            }
        }

        for (int i = 0; i < totalPage; i++)
        {
            if (Input.GetKeyDown(inputs[i]))
            {
                if (i >= 0 && i < pages[currentPage].Length)
                {
                    AudioManager.instance.Play("debug");
                    Teleport(pages[currentPage][i]);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            AudioManager.instance.Play("debug");
            currentPage++;
            currentPage %= totalPage;
            UpdateUI();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AudioManager.instance.Play("debug");
            currentPage--;
            if (currentPage == -1)
                currentPage = totalPage - 1;
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            PlayerStatus.instance.JumpRespawn();
            AudioManager.instance.Play("debug");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            AudioManager.instance.Play("debug");
            ToggleView(!isShown);
        }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            AudioManager.instance.Play("debug");
            movements.JETPACKMODE = !movements.JETPACKMODE;
        }

        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            AudioManager.instance.Play("debug");
            LoadSceneManager.instance.LoadScene(2, true);
        }
    }

    private void Teleport(Transform tp)
    {
        player.position = tp.position;
    }

    private void UpdateUI()
    {
        textOnCanvas.text = "TP Page: " + pageNames[currentPage];
    }

    private void ToggleView(bool toggle)
    {
        textOnCanvas.alpha = (toggle) ? 1 : 0;
        isShown = !isShown;
    }
}
