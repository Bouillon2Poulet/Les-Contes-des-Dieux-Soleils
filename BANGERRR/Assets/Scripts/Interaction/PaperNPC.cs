using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperNPC : Note, IInteractable
{
    [Header("Text to display")]
    public string text;
    
    public void Interact()
    {
        PaperNPCManager.instance.DisplayText(text);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
