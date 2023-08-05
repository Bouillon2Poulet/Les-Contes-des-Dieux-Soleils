using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpTriggerButton : MonoBehaviour, IInteractable
{
    public GameObject bubble;
    private bool InteractionAvailable = true;
    private float interactRange;
    private bool hasBeenTriggered = false;

    private void Start()
    {
        interactRange = FindObjectOfType<Interactor>().interactRange;
    }

    public void Interact()
    {
        if (!hasBeenTriggered)
        {
            Debug.Log("INTERACT");
            hasBeenTriggered = true;
            InteractionAvailable = false;
            ToggleBubble(false);


            AmpAscenseur.instance.TakeElevatorDown();

            
            instance = null;
        }
    }

    private void Update()
    {
        if (InteractionAvailable)
        {
            ToggleBubble(CheckPlayer());
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

    public static AmpTriggerButton instance { get; private set; }

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
    }
}
