using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            }
        } else
        {
            if (isPlayerIn)
            {
                if (FindAnyObjectByType<isSolisedeAlignedWithSolimont>().Check())
                {
                    Debug.Log("Starting the ritual");
                    FindAnyObjectByType<FadeToBlack>().FadeInBlack(1f);
                    Invoke(nameof(StartRitual), 1);
                    hasStartedRitual = true;
                    ritualPhase = 1;
                }
            }
        }
    }

    private void StartRitual()
    {
        var dialogManager = FindAnyObjectByType<DialogManager>();
        if (dialogManager.isItActive())
        {
            dialogManager.ForceEnd();
        }
        player.position = TpPont.position; // TP Joueur
        FindAnyObjectByType<FadeToBlack>().FadeOutBlack(1f); // Fade back
        FindAnyObjectByType<SystemDayCounter>().pauseSystem(); // 
        FindAnyObjectByType<NPCEventsManager>().SolDeactivateNPCs(); //
        FindAnyObjectByType<NPCEventsManager>().Sol_ritualStarted = true;
        LeVraiPont.SetActive(true);
        TriggerPont.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerIn = false;
        }
    }
}
