using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCSwimNinja : MonoBehaviour, IInteractable
{
    public bool isInteractionAllowed = true;

    [SerializeField] public GameObject bubble;
    //[SerializeField] public string npcName; // deprecated
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] private bool shouldLookAtTarget = true;

    [SerializeField] public string[] actors;

    [SerializeField] public Message[] messagesA;
    public bool isPageARead = false;

    public bool pageB;
    [SerializeField] public Message[] messagesB;
    public bool isPageBRead = false;

    public bool pageC;
    [SerializeField] public Message[] messagesC;
    public bool isPageCRead = false;

    public bool pageD;
    [SerializeField] public Message[] messagesD;
    public bool isPageDRead = false;

    public bool pageE;
    [SerializeField] public Message[] messagesE;
    public bool isPageERead = false;

    private GameObject lookAtTarget;
    private float initialBubbleSize;
    private GravityBody gb;

    public void DoPageD()
    {
        FindObjectOfType<DialogManager>().OpenDialog(messagesD, actors);
        isPageDRead = true;
        pageE = true;
        waitingToDismountFirstTime = true;
    }

    private bool waitingToMountFirstTime = false;
    private bool waitingToDismountFirstTime = false;
    private bool waitingToMountFull = false;

    public void Interact()
    {
        if (isInteractionAllowed)
        {
            if (pageE && messagesE.Length > 0)
            {
                FindObjectOfType<DialogManager>().OpenDialog(messagesE, actors);
                isPageERead = true;
                waitingToMountFull = true;
            }
            else if (pageC && messagesC.Length > 0)
            {
                AmpSwimNinjaEvents.instance.FlipNinja();
                FindObjectOfType<DialogManager>().OpenDialog(messagesC, actors);
                isPageCRead = true;
                pageD = true;
                isInteractionAllowed = false;
                waitingToMountFirstTime = true;
            }
            else if (pageB && messagesB.Length > 0)
            {
                FindObjectOfType<DialogManager>().OpenDialog(messagesB, actors);
                isPageBRead = true;
                pageC = true;
            }
            else if (messagesA.Length > 0)
            {
                FindObjectOfType<DialogManager>().OpenDialog(messagesA, actors);
                isPageARead = true;
                pageB = true;
            }
        }
    }

    public void ShowBubble()
    {
        bubble.transform.localScale = Vector3.one * initialBubbleSize;
    }

    public void HideBubble()
    {
        bubble.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        lookAtTarget = GameObject.FindGameObjectWithTag("SpritesTarget");
        gb = GetComponent<GravityBody>();
        initialBubbleSize = bubble.transform.localScale.x;
        HideBubble();
    }

    private void FixedUpdate()
    {
        if (waitingToMountFirstTime)
        {
            if (!FindObjectOfType<DialogManager>().isItActive())
            {
                waitingToMountFirstTime = false;
                AmpSwimNinjaEvents.instance.MountPlayerFirstTime();
            }
        }

        if (waitingToDismountFirstTime)
        {
            if (!FindObjectOfType<DialogManager>().isItActive())
            {
                waitingToDismountFirstTime = false;
                AmpSwimNinjaEvents.instance.DismountPlayer();
            }
        }

        if (waitingToMountFull)
        {
            if (!FindObjectOfType<DialogManager>().isItActive())
            {
                waitingToMountFull = false;
                AmpSwimNinjaEvents.instance.MountPlayerFull();
                isInteractionAllowed = false;
            }
        }

        if (shouldLookAtTarget)
        {
            sprite.transform.LookAt(lookAtTarget.transform.position, -gb.GravityDirection);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ThirdPersonMovement player) && messagesA.Length > 0)
        {
            ShowBubble();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ThirdPersonMovement player))
        {
            HideBubble();
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}