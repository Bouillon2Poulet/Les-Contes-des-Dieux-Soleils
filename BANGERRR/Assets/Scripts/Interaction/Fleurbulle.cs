using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleurbulle : MonoBehaviour, IInteractable
{
    public bool isBubbleAvailable = true;
    private PlayerStatus player;

    private Sprite originalSprite;
    private Sprite emptySprite;
    public SpriteRenderer spriteRenderer;

    private GravityBody gb;
    private GameObject lookAtTarget;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerStatus>();
        emptySprite = Resources.Load<Sprite>("fleurbulle_empty");
        originalSprite = spriteRenderer.sprite;

        lookAtTarget = GameObject.FindGameObjectWithTag("SpritesTarget");
        gb = GetComponent<GravityBody>();
    }

    private void FixedUpdate()
    {
        Vector3 upDirection = gb.GravityDirection;
        Vector3 targetPosition = lookAtTarget.transform.position;

        Vector3 forwardDirection = targetPosition - spriteRenderer.transform.position;
        Vector3 forwardProjected = Vector3.ProjectOnPlane(forwardDirection, upDirection);
        Quaternion targetRotation = Quaternion.LookRotation(forwardProjected, -upDirection);
        spriteRenderer.transform.rotation = targetRotation;
    }

    public void Interact()
    {
        Debug.Log("Fleurbulle" + GetInstanceID() + " déclenchée");
        if (isBubbleAvailable)
        {
            if (!player.hasBubbleOn)
            {
                player.WearBubble();
                giveBubble();
            }
            else
            {
                Debug.Log("Joueur a déjà une bulle");
            }
        }
        else
        {
            Debug.Log("Fleurbulle" + GetInstanceID() + " a déjà donné sa bulle");
        }
    }

    public void giveBubble()
    {
        Debug.Log("Fleurbulle" + GetInstanceID() + " donne sa bulle à joueur");
        isBubbleAvailable = false;
        spriteRenderer.sprite = emptySprite;
        GetComponent<InteractionBubble>().TurnOff();
        KeyInteractionManager.instance.ToggleActionIcon(1, false);
    }

    public void retrieveBubble()
    {
        Debug.Log("Fleurbulle" + GetInstanceID() + " récup sa bulle");
        isBubbleAvailable = true;
        spriteRenderer.sprite = originalSprite;
        GetComponent<InteractionBubble>().TurnOn();
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
