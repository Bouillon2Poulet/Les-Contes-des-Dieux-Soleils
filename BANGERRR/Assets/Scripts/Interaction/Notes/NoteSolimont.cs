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
        if (!beenSung)
        {
            if (Flegmardo.isPageDRead)
            {
                // Fonction Chanter()
                AudioManager.instance.Play("chant");
                FindAnyObjectByType<NPCEventsManager>().Soli_songSung = true;
                beenSung = true;
                murInvisible.SetActive(false);
                ///DialogManager.instance.OpenMessage("*Chanson*", "DEBUG", "Solimont");
                GetComponent<InteractionBubble>().TurnOff();
                transform.gameObject.SetActive(false);
            }
            else
            {
                DialogManager.instance.OpenMessage("Ne touche pas à ça !", "Leave that alone!", "Flegmardo", "Solimont"); ;
            }
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
