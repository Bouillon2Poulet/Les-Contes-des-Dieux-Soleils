using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmoguide : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        PlayerStatus P = FindObjectOfType<PlayerStatus>();
        if (!P.hasCosmoGuide)
        {
            P.giveCosmoguide();
            string message = "Appuyez sur C pour découvrir l'univers.";
            FindObjectOfType<DialogManager>().OpenMessage(message, "Cosmoguide");
            enabled = false;
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
