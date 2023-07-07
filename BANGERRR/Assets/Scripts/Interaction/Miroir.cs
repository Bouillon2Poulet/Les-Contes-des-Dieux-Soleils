using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miroir : MonoBehaviour, IInteractable
{
    private Animator Animator;

    public void Interact()
    {
        
        Debug.Log("Opening Mirror Eye " + GetInstanceID());
        Animator.SetTrigger("TriggerOpenEyelid");
    }

    void Start()
    {
        Animator = GetComponentInParent<Animator>();
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
