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
    private bool hasRitualStarted = false;

    private void Start()
    {
        LeVraiPont.SetActive(false);
        TriggerPont.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isPlayerIn && !hasRitualStarted)
        {
            if (FindAnyObjectByType<isSolisedeAlignedWithSolimont>().Check())
            {
                if (FindAnyObjectByType<NPCEventsManager>().Nere.isPageCRead)
                {
                    hasRitualStarted = true;
                    Debug.Log("Starting the ritual");
                    StartCoroutine(nameof(Ritual));
                }
            }
        }
    }

    private IEnumerator Ritual()
    {
        if (DialogManager.instance.isItActive())
        {
            DialogManager.instance.ForceEnd();
        }

        AudioManager.instance.FadeOut("solisede", 50);
        AudioManager.instance.FadeIn("rituel", 50);

        yield return FadeToBlack.instance.Fade(true, .3f);

        SystemDayCounter.instance.pauseSystem();
        // + kill current larme si jamais ça fait des bugs mais pour l'instant ça a l'air d'aller

        player.position = TpPont.position; // TP Joueur
        player.rotation = TpPont.rotation;

        FindAnyObjectByType<NPCEventsManager>().SolDeactivateNPCs();
        FindAnyObjectByType<NPCEventsManager>().Sol_ritualStarted = true;
        LeVraiPont.SetActive(true);
        TriggerPont.SetActive(true);

        yield return FadeToBlack.instance.Fade(false, .5f);

        yield return new WaitForSeconds(1);

        ShowRitualDialogs();
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
