using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpTriggerButton1 : MonoBehaviour, IInteractable
{
    public GameObject MurInvisible;
    public GameObject MurInvisible2;

    public Animator SolFusee;

    public void Interact()
    {
        Debug.Log("INTERACT");
        GetComponent<InteractionBubble>().TurnOff();

        // Lancer animation fusée et tt !!!
        Debug.Log("la fusée ça part !!");
        AmpAnimationFusee.instance.StartAnimation();

        MurInvisible.SetActive(false);
        MurInvisible2.SetActive(true);

        SolFusee.SetTrigger("trigger");

        Destroy(this);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
