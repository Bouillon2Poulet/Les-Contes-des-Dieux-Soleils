using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject DialogBox;
    private CanvasGroup DialogBoxGroup;
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
            isActive = true;
            currentMessages = messages;
            currentActors = actors;
            activeMessageIndex = 0;
            FindObjectOfType<ThirdPersonMovement>().blockPlayerMoveInputs();

            //Debug.Log("[DialogManager] Loaded message : " + messages.Length);
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
            isActive = false;
            FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            FindObjectOfType<NPCEventsManager>().updateNPCPages();
            backgroundBox.localScale = Vector3.zero;
            Debug.Log("[DialogManager] End of messages");
        }
    }

    public void ForceEnd()
    {
        isActive = false;
        FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        backgroundBox.localScale = Vector3.zero;
        Debug.Log("[DialogManager] FORCED End of messages");
    }

    //public IEnumerator EphemeralMessage(string name, string text)
    public IEnumerator EphemeralMessage(string name, string text, float duration)
    {
        npcNameText.text = name;
        messageText.text = text;

        float fadingSpeed = .05f;
        float fadingProgression = 0f;
        backgroundBox.localScale = Vector3.one;
        arrow.localScale = Vector3.zero;
        Debug.Log("Start Ephemeral Message");

        while (fadingProgression < 1f)
        {
            DialogBoxGroup.alpha = fadingProgression;
            fadingProgression += fadingSpeed;
            yield return null;
        }
        DialogBoxGroup.alpha = 1f;

        yield return new WaitForSeconds(duration);

        while (fadingProgression > 0f)
        {
            DialogBoxGroup.alpha = fadingProgression;
            fadingProgression -= fadingSpeed;
            yield return null;
        }
        DialogBoxGroup.alpha = 0f;

        backgroundBox.localScale = Vector3.zero;
        arrow.localScale = Vector3.one;
        Debug.Log("End Ephemeral Message");
    }

    private void Start()
    {
        backgroundBox.localScale = Vector3.zero;
        DialogBoxGroup = backgroundBox.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (isActive == true && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (currentMessages != null)
            {
                NextMessage();
            }
        }
        //Debug.Log(currentMessages);
    }

    private void Awake()
    {
        DialogBox.SetActive(true);
    }

    public bool isItActive()
    {
        return isActive;
    }
}
