using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCtest : MonoBehaviour, IInteractable
{
    [SerializeField] public string npcName;
    [SerializeField] public string[] messages;
    [SerializeField] public GameObject bubble;
    private bool hasSomethingToSay;

    public void Interact()
    {
        if (hasSomethingToSay)
        {
            FindObjectOfType<DialogManager>().OpenDialog(messages, npcName);
        }
    }

    public void ShowBubble()
    {
        bubble.transform.localScale = Vector3.one * .145f;
    }

    public void HideBubble()
    {
        bubble.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        HideBubble();
        if (messages.Length > 0)
        {
            hasSomethingToSay = true;
        }
        else
        {
            hasSomethingToSay = false;
        }
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

