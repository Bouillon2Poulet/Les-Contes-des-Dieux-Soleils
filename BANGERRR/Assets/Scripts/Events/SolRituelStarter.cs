using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SolRituelStarter : MonoBehaviour
{
    public Rigidbody player;
    public Transform TpPont;
    public GameObject LeVraiPont;
    public GameObject TriggerPont;

    private bool isPlayerIn = false;
    private bool hasStartedRitual = false;
    private int ritualPhase = 0;

    private void Start()
    {
        LeVraiPont.SetActive(false);
        TriggerPont.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (hasStartedRitual)
        {
            if (ritualPhase == 1)
            {
                Debug.Log("Jme kill");
                transform.gameObject.SetActive(false);
            }
        } 
        else
        {
            if (isPlayerIn)
            {
                if (FindAnyObjectByType<isSolisedeAlignedWithSolimont>().Check())
                {
                    if (FindAnyObjectByType<NPCEventsManager>().Nere.isPageCRead)
                    {
                        Debug.Log("Starting the ritual");
                        FindAnyObjectByType<FadeToBlack>().FadeInBlack(1f);
                        Invoke(nameof(StartRitual), 1);
                        hasStartedRitual = true;
                        ritualPhase = 1;
                    }
                    else
                    {
                        Debug.Log("t'as pas encore tout fait mon reuf");
                    }
                }
            }
        }
    }

    private void StartRitual()
    {
        if (DialogManager.instance.isItActive())
        {
            DialogManager.instance.ForceEnd();
        }
        player.position = TpPont.position; // TP Joueur
        FindAnyObjectByType<FadeToBlack>().FadeOutBlack(1f); // Fade back
        FindAnyObjectByType<SystemDayCounter>().pauseSystem(); // 
        FindAnyObjectByType<NPCEventsManager>().SolDeactivateNPCs(); //
        FindAnyObjectByType<NPCEventsManager>().Sol_ritualStarted = true;
        LeVraiPont.SetActive(true);
        TriggerPont.SetActive(true);
        Invoke(nameof(ShowRitualDialogs), 1);
    }

    private void ShowRitualDialogs()
    {
        NPCEventsManager NPCs = FindAnyObjectByType<NPCEventsManager>();
        Message[] messagesOkaoka = NPCs.Okaoka.messagesC;
        messagesOkaoka[0].actorID = 1;
        Message[] messagesNere = NPCs.Nere.messagesD;
        Message[] combinedMessages = messagesNere.Concat(messagesOkaoka).ToArray();
        string[] actors = { "Nere", "Okaoka" };
        DialogManager.instance.OpenDialog(combinedMessages, actors, "Solisede");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerIn = false;
        }
    }
}
