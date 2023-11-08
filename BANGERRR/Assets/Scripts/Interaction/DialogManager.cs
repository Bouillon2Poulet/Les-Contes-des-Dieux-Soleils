using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    private CanvasGroup DialogBoxGroup;
    private TextMeshProUGUI npcNameText;
    private TextMeshProUGUI messageText;
    private RectTransform backgroundBox;
    private RectTransform arrow;

    private Message[] currentMessages;
    private string[] currentActors;
    private int activeMessageIndex = 0;
    public static bool isActive = false;
    public bool ephemeralMessageGoing = false;

    bool isTyping = false;
    char previousLetter = ' ';
    CanvasGroup arrowOpacity;
    [Header("Typing")]
    public float wordBaseSpeed = .012f;
    public float wordSpeedDivider = 10f;

    float wordSpeed;
    float wordSpeedFast;

    Vector3 normalDialogBoxScale;
    Vector3 hiddenDialogBoxScale;

    [Header("Dialog Skins")]
    public GameObject SkinFolder;
    public GameObject Neutre;
    public GameObject Fin;
    public GameObject Oeil;
    public GameObject Amphipolis;
    public GameObject SoleilRouge;
    public GameObject Solimont;
    public GameObject Solisede;

    private void InitSkin(string skinName)
    {
        GameObject currentSkin = Neutre;
        if (skinName == "Fin")
            currentSkin = Fin;
        else if (skinName == "Oeil")
            currentSkin = Oeil;
        else if (skinName == "Amphipolis")
            currentSkin = Amphipolis;
        else if (skinName == "SoleilRouge")
            currentSkin = SoleilRouge;
        else if (skinName == "Solimont")
            currentSkin = Solimont;
        else if (skinName == "Solisede")
            currentSkin = Solisede;

        DialogBoxGroup = currentSkin.GetComponent<CanvasGroup>();
        npcNameText = currentSkin.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        messageText = currentSkin.transform.Find("Message").GetComponent<TextMeshProUGUI>();
        backgroundBox = currentSkin.GetComponent<RectTransform>();
        arrow = currentSkin.transform.Find("Arrow").GetComponent<RectTransform>();
        arrowOpacity = currentSkin.transform.Find("Arrow").GetComponent<CanvasGroup>();
    }

    public void OpenDialog(Message[] messages, string[] actors)
    {
        OpenDialog(messages, actors, "null");
    }

    public void OpenDialog(Message[] messages, string[] actors, string skin)
    {
        InitSkin(skin);

        if (!isActive)
        {
            isActive = true;
            currentMessages = messages;
            currentActors = actors;
            activeMessageIndex = 0;
            FindObjectOfType<ThirdPersonMovement>().blockPlayerMoveInputs();

            //Debug.Log("[DialogManager] Loaded message : " + messages.Length);
            backgroundBox.localScale = normalDialogBoxScale;
            StartCoroutine(DisplayMessage());
            arrowOpacity.alpha = 0;
            if (messages.Length > 1)
            {
                arrow.localScale = Vector3.one;
            }
            else
            {
                arrow.localScale = Vector3.zero;
            }
        }
    }

    public void OpenMessage(string text, string name, string skin)
    {
        InitSkin(skin);

        Message uniqueMessage = new Message
        {
            message = text,
            actorID = 0
        };
        Message[] messages = { uniqueMessage };
        string[] actors = { name };
        OpenDialog(messages, actors, skin);
    }

    IEnumerator DisplayMessage()
    {
        isTyping = true;
        messageText.text = "";

        Message messageToDisplay = currentMessages[activeMessageIndex];
        //messageText.text = messageToDisplay.message;
        npcNameText.text = currentActors[messageToDisplay.actorID];

        foreach(char letter in messageToDisplay.message.ToCharArray())
        {
            //Debug.Log(wordSpeed);
            if (previousLetter == '.')
            {
                if (letter != '.')
                    yield return new WaitForSeconds(wordSpeed * 20f);
                else
                    yield return new WaitForSeconds(wordSpeed * 40f);
            }
            else if (previousLetter == '?' || previousLetter == '!')
            {
                if (letter != '!' && letter != '?')
                    yield return new WaitForSeconds(wordSpeed * 15f);
            }
            else if (previousLetter == ',' || previousLetter == ':')
                yield return new WaitForSeconds(wordSpeed * 10f);

            messageText.text += letter;
            previousLetter = letter;
            if (wordSpeed != wordSpeedFast)
                yield return new WaitForSeconds(wordSpeed);
        }

        previousLetter = ' ';
        arrowOpacity.alpha = 1;
        isTyping = false;
    }

    public void NextMessage()
    {
        activeMessageIndex++;
        arrowOpacity.alpha = 0;

        if (activeMessageIndex < currentMessages.Length)
        {
            if (activeMessageIndex == currentMessages.Length-1)
            {
                arrow.localScale = hiddenDialogBoxScale;
            }
            StartCoroutine(DisplayMessage());
        }
        else
        {
            isActive = false;
            FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            FindObjectOfType<NPCEventsManager>().updateNPCPages();
            backgroundBox.localScale = hiddenDialogBoxScale;
            Debug.Log("[DialogManager] End of messages");
        }
    }

    public void ForceEnd()
    {
        isActive = false;
        FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        backgroundBox.localScale = hiddenDialogBoxScale;
        Debug.Log("[DialogManager] FORCED End of messages");
    }


    public IEnumerator EphemeralMessage(string name, string text, float duration)
    {
        StartCoroutine(EphemeralMessage(name, text, duration, "Oeil"));
        yield return null;
    }

    public IEnumerator EphemeralMessage(string name, string text, float duration, string skin)
    {
        InitSkin(skin);

        ephemeralMessageGoing = true;

        npcNameText.text = name;
        messageText.text = text;

        float fadingSpeed = .08f;
        float fadingProgression = 0f;
        backgroundBox.localScale = Vector3.one;
        arrow.localScale = Vector3.zero;

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
        ephemeralMessageGoing = false;
    }

    private void Start()
    {
        normalDialogBoxScale = Neutre.GetComponent<RectTransform>().localScale;
        hiddenDialogBoxScale = Vector3.zero;

        wordSpeed = wordBaseSpeed;
        wordSpeedFast = wordBaseSpeed / wordSpeedDivider;

        foreach (Transform child in SkinFolder.transform)
        {
            child.GetComponent<RectTransform>().localScale = hiddenDialogBoxScale;
        }

        InitSkin("none");
    }

    void Update()
    {
        if (isActive == true && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (currentMessages != null && !isTyping)
            {
                NextMessage();
            }

            //wordSpeed = wordSpeedFast;
        }

        /*if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            wordSpeed = wordBaseSpeed;
        }*/
    }

    public void DialoguesRapides(bool state)
    {
        wordSpeed = (state) ? wordSpeedFast : wordBaseSpeed;
    }

    public bool isItActive()
    {
        return isActive;
    }

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

        SkinFolder.SetActive(true);
    }
    public static DialogManager instance { get; private set; }
}
