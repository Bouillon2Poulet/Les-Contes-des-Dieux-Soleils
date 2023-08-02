using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaperNPCManager : MonoBehaviour
{
    public GameObject PaperBox;
    public TextMeshProUGUI textArea;

    private bool isActive = false;

    private void Start()
    {
        PaperBox.SetActive(true);
        CloseText();
    }

    public void DisplayText(string textToDisplay)
    {
        isActive = true;
        textArea.text = textToDisplay;
        TogglePaperBox(true);

        FindObjectOfType<ThirdPersonMovement>().blockPlayerMoveInputs();
    }

    private void TogglePaperBox(bool state)
    {
        PaperBox.transform.localScale = (state) ? Vector3.one : Vector3.zero;
    }

    private void CloseText()
    {
        isActive = false;
        textArea.text = "";
        TogglePaperBox(false);
        FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
    }

    private void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                CloseText();
            }
        }
    }

    // Singleton
    public static PaperNPCManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
