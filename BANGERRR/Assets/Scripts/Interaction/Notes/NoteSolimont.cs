using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSolimont : Note, IInteractable
{
    public NPC Flegmardo;
    public GameObject murInvisible;

    private bool beenSung;

    [Header("Bubble stuff")]
    public GameObject bubble;
    private float interactRange;
    public Rigidbody player;

    private void Start()
    {
        interactRange = FindObjectOfType<Interactor>().interactRange;
    }

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
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return (distance < interactRange+1);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
