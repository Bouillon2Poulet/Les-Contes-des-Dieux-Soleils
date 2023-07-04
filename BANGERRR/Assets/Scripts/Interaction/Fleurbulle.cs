using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleurbulle : MonoBehaviour, IInteractable
{
    public bool isBubbleAvailable = true;
    private PlayerStatus player;

    private Sprite originalSprite;
    private Sprite emptySprite;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindAnyObjectByType<PlayerStatus>();
        emptySprite = Resources.Load<Sprite>("fleurbulle_empty");
        originalSprite = spriteRenderer.sprite;
    }

    public void Interact()
    {
        Debug.Log("Fleurbulle" + GetInstanceID() + " d�clench�e");
        if (isBubbleAvailable)
        {
            if (!player.hasBubbleOn)
            {
                player.WearBubble();
                giveBubble();
            }
            else
            {
                Debug.Log("Joueur a d�j� une bulle");
            }
        }
        else
        {
            Debug.Log("Fleurbulle" + GetInstanceID() + " a d�j� donn� sa bulle");
        }
    }

    public void giveBubble()
    {
        Debug.Log("Fleurbulle" + GetInstanceID() + " donne sa bulle � joueur");
        isBubbleAvailable = false;
        spriteRenderer.sprite = emptySprite;
    }

    public void retrieveBubble()
    {
        Debug.Log("Fleurbulle" + GetInstanceID() + " r�cup sa bulle");
        isBubbleAvailable = true;
        spriteRenderer.sprite = originalSprite;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
