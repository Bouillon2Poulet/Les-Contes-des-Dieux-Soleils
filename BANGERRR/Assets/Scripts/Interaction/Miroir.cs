using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miroir : MonoBehaviour, IInteractable
{
    private Animator Animator;
    public bool hasBeenTriggered;

    public void Interact()
    {
        if (!hasBeenTriggered)
        {
            hasBeenTriggered = true;
            Debug.Log("Opening Mirror Eye " + GetInstanceID());
            Animator.SetTrigger("TriggerOpenEyelid");
        }
    }

    void Start()
    {
        Animator = GetComponentInParent<Animator>();
        hasBeenTriggered = false;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
