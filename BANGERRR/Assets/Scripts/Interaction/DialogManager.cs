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

    private Message[] currentMessages;
    private string[] currentActors;
    private int activeMessageIndex = 0;
    public static bool isActive = false;

    public void OpenDialog(Message[] messages, string[] actors)
    {
        if (!isActive)
        {
            currentMessages = messages;
            currentActors = actors;
            activeMessageIndex = 0;
            isActive = true;
            FindObjectOfType<ThirdPersonMovement>().blockPlayerMoveInputs();

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

    public void OpenMessage(string text, string name)
    {
        Message uniqueMessage = new Message
        {
            message = text,
            actorID = 0
        };
        Message[] messages = { uniqueMessage };
        string[] actors = { name };
        OpenDialog(messages, actors);
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessageIndex];
        messageText.text = messageToDisplay.message;
        npcNameText.text = currentActors[messageToDisplay.actorID];
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
            FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
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
