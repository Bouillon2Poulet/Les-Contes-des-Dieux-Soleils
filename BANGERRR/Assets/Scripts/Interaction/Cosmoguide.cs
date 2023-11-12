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
            string message = "Un bien �trange art�fact� Ses secrets semblent avoir r�ussi � affronter l��preuve du temps. ";
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
