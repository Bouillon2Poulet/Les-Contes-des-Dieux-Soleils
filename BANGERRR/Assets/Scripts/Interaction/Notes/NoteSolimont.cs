using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSolimont : Note, IInteractable
{
    public NPC Flegmardo;
    public GameObject murInvisible;

    private bool beenSung;

    public void Interact()
    {
        Debug.Log("Salut c'est la note de Solimont !");
        if (!beenSung)
        {
            if (Flegmardo.isPageDRead)
            {
                // Fonction Chanter()
                FindAnyObjectByType<NPCEventsManager>().Soli_songSung = true;
                beenSung = true;
                murInvisible.SetActive(false);
                FindObjectOfType<DialogManager>().OpenMessage("*Chanson*", "DEBUG", "Solimont");
            }
            else
            {
                FindObjectOfType<DialogManager>().OpenMessage("Ne touche pas à ça !", "Flegmardo", "Solimont");
            }
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
