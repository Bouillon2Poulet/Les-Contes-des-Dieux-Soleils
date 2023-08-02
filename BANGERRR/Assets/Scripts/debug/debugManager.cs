using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class debugManager : MonoBehaviour
{
    public TextMeshProUGUI textOnCanvas;
    public Rigidbody player;
    [Header("TPs")]
    [SerializeField] public Transform[] Triton;
    [SerializeField] public Transform[] EauxDivines;
    [SerializeField] public Transform[] Solisede;
    [SerializeField] public Transform[] Solimont;
    [SerializeField] public Transform[] Amphipolis;
    [SerializeField] public Transform[] Larme;

    public int currentPage = 0;

    private int totalPage = 0;
    private List<Transform[]> pages;
    private KeyCode[] inputs;
    private string[] pageNames;
    private bool isShown = true;

    private void Start()
    {
        pageNames = new string[] { "Triton", "Eaux Divines", "Solisède", "Solimont", "Amphipolis", "Larme" };
        pages = new List<Transform[]>
        {
            Triton,
            EauxDivines,
            Solisede,
            Solimont,
            Amphipolis,
            Larme
        };

        totalPage = pages.Count;
        UpdateUI();

        inputs = new KeyCode[] { KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9 };
    }

    private void Update()
    {
        for (int i = 0; i < totalPage; i++)
        {
            if (Input.GetKeyDown(inputs[i]))
            {
                Teleport(pages[currentPage][i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            currentPage++;
            currentPage %= totalPage;
            UpdateUI();
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            currentPage--;
            if (currentPage == -1)
                currentPage = totalPage - 1;
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            FindObjectOfType<PlayerStatus>().JumpRespawn();
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            ToggleView(!isShown);
        }
    }

    private void Teleport(Transform tp)
    {
        player.position = tp.position;
    }

    private void UpdateUI()
    {
        textOnCanvas.text = pageNames[currentPage];
    }

    private void ToggleView(bool toggle)
    {
        textOnCanvas.alpha = (toggle) ? 1 : 0;
        isShown = !isShown;
    }
}
