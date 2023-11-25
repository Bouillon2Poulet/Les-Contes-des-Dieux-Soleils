using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmoguide : Note, IInteractable
{
    public void Interact()
    {
        if (!PlayerStatus.instance.hasCosmoGuide)
        {
            AudioManager.instance.Play("pickup");
            PlayerStatus.instance.giveCosmoguide();
            string message = "Un bien étrange artéfact… Ses secrets semblent avoir réussi à affronter l’épreuve du temps.";
            string engMessage = "A quite strange artifact... Its secrets seem to have stood the test of time.";
            DialogManager.instance.OpenMessage(message, engMessage, "Cosmoguide", "Neutre");
            GetComponent<InteractionBubble>().ToggleActionIcon(false);
            transform.gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
