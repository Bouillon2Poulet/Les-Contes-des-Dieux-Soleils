using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperNPC : Note, IInteractable
{
    [Header("Bubble stuff")]
    public GameObject bubble;
    private float interactRange;

    [Header("Text to display")]
    public string text;
    
    public void Interact()
    {
        PaperNPCManager.instance.DisplayText(text);
    }


    private void Start()
    {
        interactRange = FindObjectOfType<Interactor>().interactRange;
    }

    private void Update()
    {
        if (CheckPlayer())
        {
            ToggleBubble(true);
        }
        else
        {
            ToggleBubble(false);
        }
    }

    public void ToggleBubble(bool state)
    {
        bubble.SetActive(state);
    }

    private bool CheckPlayer()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }


    public Transform GetTransform()
    {
        return transform;
    }
}