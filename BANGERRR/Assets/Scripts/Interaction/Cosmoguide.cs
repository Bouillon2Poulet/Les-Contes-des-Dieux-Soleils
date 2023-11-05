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
            string message = "Vous trouvez un CosmoGuide ! Appuyez sur C pour d�couvrir l'univers.";
            FindObjectOfType<DialogManager>().OpenMessage(message, "Objet trouv�", "Neutre");
            GetComponent<InteractionBubble>().ToggleActionIcon(false);
            transform.gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
