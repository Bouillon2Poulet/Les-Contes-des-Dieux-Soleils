using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject bubble;
    [SerializeField] public string npcName;
    [SerializeField] public SpriteRenderer sprite;
    
    [SerializeField] public string[] actors;

    [SerializeField] public Message[] messagesA;
    public bool pageB;
    [SerializeField] public Message[] messagesB;
    public bool pageC;
    [SerializeField] public Message[] messagesC;
    public bool pageD;
    [SerializeField] public Message[] messagesD;
    public bool pageE;
    [SerializeField] public Message[] messagesE;

    private GameObject lookAtTarget;
    private float initialBubbleSize;
    private GravityBody gb;

    public void Interact()
    {
        if (pageE && messagesE.Length > 0)
        {
            FindObjectOfType<DialogManager>().OpenDialog(messagesE, actors);
        }
        else if (pageD && messagesD.Length > 0)
        {
            FindObjectOfType<DialogManager>().OpenDialog(messagesD, actors);
        }
        else if (pageC && messagesC.Length > 0)
        {
            FindObjectOfType<DialogManager>().OpenDialog(messagesC, actors);
        }
        else if (pageB && messagesB.Length > 0)
        {
            FindObjectOfType<DialogManager>().OpenDialog(messagesB, actors);
        } 
        else if (messagesA.Length > 0)
        {
            FindObjectOfType<DialogManager>().OpenDialog(messagesA, actors);
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
        sprite.transform.LookAt(lookAtTarget.transform.position, -gb.GravityDirection);
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

[System.Serializable]
public class Message
{
    public int actorID;
    public string message;
}