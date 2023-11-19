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
            AudioManager.instance.Play("concrete");
            Animator.SetTrigger("TriggerOpenEyelid");
        }

        if (!hasFocus)
        {
            miroirCam.Priority = 100;
            hasFocus = true;
            FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
            FindAnyObjectByType<MainCameraManager>().blockMovement();
            PlayerStatus.instance.hideSprite();
            GetComponent<InteractionBubble>().ToggleBubble(false);
        }
        else
        {
            miroirCam.Priority = 0;
            PlayerStatus.instance.showSprite();
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
        FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        FindAnyObjectByType<MainCameraManager>().unblockMovement();
        GetComponent<InteractionBubble>().ToggleBubble(true);
    }
}
