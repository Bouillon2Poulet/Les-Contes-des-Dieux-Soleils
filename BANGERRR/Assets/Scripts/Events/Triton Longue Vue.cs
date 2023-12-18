using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TritonLongueVue : MonoBehaviour, IInteractable
{
    public GameObject TelescopeImageMask;
    public CinemachineVirtualCamera TelescopeCamera;
    private bool lookingThroughTelescope = false;

    private ThirdPersonMovement playerMovement;

    public GameObject larme;
    public Transform larmeStart;
    public Transform larmeLanding;
    private bool larmeAnimationHasStarted = false;
    private float animationProgress = -1f;
    public float animationSpeed = .001f;
    public bool animationCanStart = true;

    public void Interact()
    {
        if (!larmeAnimationHasStarted && animationCanStart)
        {
            StartLarmeAnimation();
            GetComponent<InteractionBubble>().ToggleActionIcon(false);
            AudioManager.instance.Play("scope");
            StartCoroutine(nameof(DelayedMessage));
        } 
        else if (!larmeAnimationHasStarted)
        {
            BasicInteractTelescope();
        }
    }

    IEnumerator DelayedMessage()
    {
        yield return new WaitForSecondsRealtime(3f);
        StartCoroutine(DialogManager.instance.EphemeralMessage(
            "Triton",
            "Aujourd'hui encore l'univers semble tranquille, tout comme ma vie l'était hier et comme elle le sera surement demain. Parfois je me demande ce qui se cache derrière le soleil, puis j'arrête vite car la curiosité est un vilain défaut...",
            "The universe still seems tranquil today, just as my life was yesterday and will surely be tomorrow. Sometimes I wonder what's behind the sun, but then I quickly stop, because curiosity is a bad habit...",
            10, "Neutre"
        ));
        yield return null;
    }

    private void BasicInteractTelescope()
    {
        if (lookingThroughTelescope)
        {
            playerMovement.unblockPlayerMoveInputs();
            TelescopeCamera.Priority = 1;
            TelescopeImageMask.SetActive(!TelescopeImageMask.activeSelf);
        }
        else
        {
            playerMovement.blockPlayerMoveInputs();
            TelescopeCamera.Priority = 100;
            TelescopeImageMask.SetActive(!TelescopeImageMask.activeSelf);
        }
        lookingThroughTelescope = !lookingThroughTelescope;
    }

    private void FixedUpdate()
    {
        if (larmeAnimationHasStarted && animationProgress < 1.01)
        {
            Vector3 larmeStartPos = larmeStart.position;
            Vector3 larmeLandingPos = larmeLanding.position;
            larme.transform.position = Vector3.Lerp(larmeStartPos, larmeLandingPos, animationProgress);
            animationProgress += animationSpeed;
            if (animationProgress >= 1.01)
            {
                TelescopeCamera.Priority = 1;
                TelescopeImageMask.SetActive(false);
                playerMovement.unblockPlayerMoveInputs();

                AudioManager.instance.Play("gone");

                FindAnyObjectByType<EDTransitionScript>().StartAnimation();
            }
        }
    }

    private void StartLarmeAnimation()
    {
        larmeAnimationHasStarted = true; 
        
        playerMovement.blockPlayerMoveInputs();
        PlayerStatus.instance.isAnimated = true;

        TelescopeCamera.Priority = 100;
        TelescopeImageMask.SetActive(true);
        KeyInteractionManager.instance.gameObject.SetActive(false);

        larme.SetActive(true);
    }

    private void Awake()
    {
        playerMovement = FindAnyObjectByType<ThirdPersonMovement>();
        larme.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
