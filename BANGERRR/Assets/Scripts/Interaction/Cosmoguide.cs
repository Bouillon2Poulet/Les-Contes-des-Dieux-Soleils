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
            string message = "Vous trouvez un CosmoGuide ! Appuyez sur C pour découvrir l'univers.";
            FindObjectOfType<DialogManager>().OpenMessage(message, "Objet trouvé");
            enabled = false;
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
