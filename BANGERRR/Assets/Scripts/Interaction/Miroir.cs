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

    bool justClosed = false;
    bool justOpened = false;

    public void Interact()
    {
        if (!hasBeenTriggered)
        {
            hasBeenTriggered = true;
            Debug.Log("Opening Mirror Eye " + GetInstanceID());
            AudioManager.instance.Play("concrete");
            Animator.SetTrigger("TriggerOpenEyelid");
        }

        if (!hasFocus && !justClosed)
        {
            miroirCam.Priority = 100;
            hasFocus = true;
            FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
            FindAnyObjectByType<MainCameraManager>().blockMovement();
            PlayerStatus.instance.hideSprite();
            GetComponent<InteractionBubble>().ToggleBubble(false);
            justOpened = true;
            StartCoroutine(nameof(DisableJustOpened));
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && hasFocus && !justOpened)
        {
            miroirCam.Priority = 0;
            PlayerStatus.instance.showSprite();
            Invoke(nameof(unblockEvent), 3);
            justClosed = true;
            StartCoroutine(nameof(DisableJustClosed));
        }
    }

    private void unblockEvent()
    {
        hasFocus = false;
        FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        FindAnyObjectByType<MainCameraManager>().unblockMovement();
        GetComponent<InteractionBubble>().ToggleBubble(true);
    }

    IEnumerator DisableJustClosed()
    {
        yield return new WaitForSecondsRealtime(.1f);
        justClosed = false;
    }

    IEnumerator DisableJustOpened()
    {
        yield return new WaitForSecondsRealtime(.1f);
        justOpened = false;
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
