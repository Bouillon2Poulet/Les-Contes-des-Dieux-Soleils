using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliRocher : MonoBehaviour, IInteractable
{
    private bool pickedByPlayer = false;

    public void Interact()
    {
        if (!pickedByPlayer)
        {
            Pick();
        }
    }

    private void Pick()
    {
        pickedByPlayer = true;
        AudioManager.instance.Play("rock");
        GetComponent<InteractionBubble>().TurnOff();
        FindAnyObjectByType<NPCEventsManager>().Soli_caillouRobbed = true;
        FindAnyObjectByType<SoliRejeteur>().gameObject.SetActive(false);
        FindAnyObjectByType<SoliRocher>().gameObject.SetActive(false);
        FindAnyObjectByType<PlayerStatus>().PutRockOnHead();
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
