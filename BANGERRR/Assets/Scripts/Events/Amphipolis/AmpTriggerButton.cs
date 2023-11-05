using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpTriggerButton : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("INTERACT");
        GetComponent<InteractionBubble>().TurnOff();

        AmpAscenseur.instance.TakeElevatorDown();

        Destroy(this);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
