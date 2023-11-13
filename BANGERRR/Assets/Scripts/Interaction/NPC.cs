using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    public string skin = "null";

    [SerializeField] public GameObject bubble;
    //[SerializeField] public string npcName; // deprecated
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] private bool shouldLookAtTarget = true;
    [SerializeField] private Vector3 lookAtOffset;

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

    public void Interact()
    {
        if (pageE && messagesE.Length > 0)
        {
            FindAnyObjectByType<DialogManager>().OpenDialog(messagesE, actors, skin);
            isPageERead = true;
        }
        else if (pageD && messagesD.Length > 0)
        {
            FindAnyObjectByType<DialogManager>().OpenDialog(messagesD, actors, skin);
            isPageDRead = true;
        }
        else if (pageC && messagesC.Length > 0)
        {
            FindAnyObjectByType<DialogManager>().OpenDialog(messagesC, actors, skin);
            isPageCRead = true;
        }
        else if (pageB && messagesB.Length > 0)
        {
            FindAnyObjectByType<DialogManager>().OpenDialog(messagesB, actors, skin);
            isPageBRead = true;
        }
        else if (messagesA.Length > 0)
        {
            FindAnyObjectByType<DialogManager>().OpenDialog(messagesA, actors, skin);
            isPageARead = true;
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
        if (shouldLookAtTarget)
        {
            Vector3 upDirection = gb.GravityDirection;
            Vector3 targetPosition = lookAtTarget.transform.position;

            Vector3 forwardDirection = targetPosition - sprite.transform.position;
            Vector3 forwardProjected = Vector3.ProjectOnPlane(forwardDirection, upDirection);
            Quaternion targetRotation = Quaternion.LookRotation(forwardProjected, -upDirection);
            sprite.transform.rotation = targetRotation;

            /* // Old code just to remember the good times
            //Vector3 upDirection = GetComponent<Rigidbody>().transform.up.normalized * -1;

            // old old
            //sprite.transform.LookAt(lookAtTarget.transform.position, -gb.GravityDirection);

            // works but sometimes upside down
            //sprite.transform.LookAt(Vector3.ProjectOnPlane(targetPosition, -upDirection), upDirection);
            //sprite.transform.Rotate(new Vector3(270f, 0f, 0f));
            //sprite.transform.Rotate(new Vector3(90f, 90f, -90f));

            //sprite.transform.Rotate(lookAtOffset);
            */
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ThirdPersonMovement player) && messagesA.Length > 0)
        {
            ShowBubble();
            KeyInteractionManager.instance.ToggleActionIcon(2, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ThirdPersonMovement player))
        {
            HideBubble();
            KeyInteractionManager.instance.ToggleActionIcon(2, false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

[System.Serializable]
public class Message
{
    public int actorID;
    public string message;
}