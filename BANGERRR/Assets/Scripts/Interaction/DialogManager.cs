using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;
    public RectTransform arrow;

    private string[] currentMessages;
    private int activeMessageIndex = 0;
    public static bool isActive = false;

    public void OpenDialog(string[] messages, string npcName)
    {
        if (!isActive)
        {
            npcNameText.text = npcName;

            currentMessages = messages;
            activeMessageIndex = 0;
            isActive = true;

            Debug.Log("[DialogManager] Loaded message : " + messages.Length);
            DisplayMessage();
            backgroundBox.localScale = Vector3.one;
            if (messages.Length > 1)
            {
                arrow.localScale = Vector3.one;
            } else
            {
                arrow.localScale = Vector3.zero;
            }
        }
    }

    void DisplayMessage()
    {
        messageText.text = currentMessages[activeMessageIndex];
    }

    public void NextMessage()
    {
        activeMessageIndex++;

        if (activeMessageIndex < currentMessages.Length)
        {
            if (activeMessageIndex == currentMessages.Length-1)
            {
                arrow.localScale = Vector3.zero;
            }
            DisplayMessage();
        }
        else
        {
            Debug.Log("[DialogManager] End of messages");
            isActive = false;
            backgroundBox.localScale = Vector3.zero;
        }
    }

    private void Start()
    {
        backgroundBox.localScale = Vector3.zero;
    }

    void Update()
    {
        if (isActive == true && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            NextMessage();
        }
    }

}
