using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Miroir : MonoBehaviour, IInteractable
{
    public bool hasBeenTriggered;
    public CinemachineVirtualCamera miroirCam;

    private Animator Animator;
    private bool hasFocus = false;

    public void Interact()
    {
        if (!hasBeenTriggered)
        {
            hasBeenTriggered = true;
            Debug.Log("Opening Mirror Eye " + GetInstanceID());
            Animator.SetTrigger("TriggerOpenEyelid");
        }

        if (!hasFocus)
        {
            miroirCam.Priority = 100;
            hasFocus = true;
            FindObjectOfType<ThirdPersonMovement>().blockPlayerMoveInputs();
            FindObjectOfType<MainCameraManager>().blockMovement();
            FindObjectOfType<PlayerStatus>().hideSprite();
            GetComponent<InteractionBubble>().ToggleBubble(false);
        }
        else
        {
            miroirCam.Priority = 0;
            FindObjectOfType<PlayerStatus>().showSprite();
            Invoke(nameof(unblockEvent), 3);
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

    private void unblockEvent()
    {
        hasFocus = false;
        FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        FindObjectOfType<MainCameraManager>().unblockMovement();
        GetComponent<InteractionBubble>().ToggleBubble(true);
    }
}
