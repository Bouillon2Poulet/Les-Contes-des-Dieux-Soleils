using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpSoleilRougeDialogue : MonoBehaviour
{
    bool isPlayerHere = false;
    bool isCooldownSet = false;
    int cooldown = 0;
    string[] messages = { "DEGAGE", "VAS T'EN", "C'EST TROP TARD", "C'EST LA FIN", "JE SUIS UNIQUE", "OBEIS MOI" };

    private void FixedUpdate()
    {
        if (isPlayerHere && !isCooldownSet)
        {
            isCooldownSet = true;
            cooldown = Random.Range(20, 50);
            StartCoroutine(MessageFromSoleilRouge(cooldown));
        }
    }

    private IEnumerator MessageFromSoleilRouge(int cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        isCooldownSet = false;

        if (!DialogManager.instance.isItActive() && !OpenCosmoGuide.instance.CosmoGuideIsOpen && !PlayerStatus.instance.isAnimated)
        {
            DialogManager.instance.OpenMessage(messages[Random.Range(0, messages.Length)], "???", "SoleilRouge");
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerHere = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerHere = false;
        }
    }

    public static AmpSoleilRougeDialogue instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
