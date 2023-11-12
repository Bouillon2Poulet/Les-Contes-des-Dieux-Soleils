using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmoguide : Note, IInteractable
{
    public void Interact()
    {
        PlayerStatus P = FindObjectOfType<PlayerStatus>();
        if (!P.hasCosmoGuide)
        {
            P.giveCosmoguide();
            string message = "Un bien étrange artéfact… Ses secrets semblent avoir réussi à affronter l’épreuve du temps. ";
            FindObjectOfType<DialogManager>().OpenMessage(message, "Cosmoguide", "Neutre");
            GetComponent<InteractionBubble>().ToggleActionIcon(false);
            transform.gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
