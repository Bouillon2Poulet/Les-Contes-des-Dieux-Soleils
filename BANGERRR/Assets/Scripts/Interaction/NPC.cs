using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject bubble;
    [SerializeField] public string npcName;
    [SerializeField] public string[] messages;
    [SerializeField] public SpriteRenderer sprite;

    private GameObject lookAtTarget;
    private float initialBubbleSize;
    private bool hasSomethingToSay;
    private GravityBody gb;

    public void Interact()
    {
        if (hasSomethingToSay)
        {
            FindObjectOfType<DialogManager>().OpenDialog(messages, npcName);
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
        hasSomethingToSay = messages.Length > 0;
    }

    private void FixedUpdate()
    {
        sprite.transform.LookAt(lookAtTarget.transform.position, -gb.GravityDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ThirdPersonMovement player) && hasSomethingToSay)
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

