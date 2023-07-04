using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool hasBubbleOn = false;
    public bool isDead = false;

    public float suffocationCountdown = 10f;
    public float initialBlinkInterval = 1f;
    public Color blinkColor = Color.red;
    private Color originalColor;
    private bool isBlinking;
    private float blinkTimer;

    private Sprite withBubbleSprite;
    private Sprite originalSprite;
    public SpriteRenderer spriteRenderer;

    private Vector3 respawnPointPosition;
    private Quaternion respawnPointRotation;

    private GravityBody gBody;

    public void WearBubble()
    {
        Debug.Log("Joueur porte une bulle");
        transform.GetPositionAndRotation(out respawnPointPosition, out respawnPointRotation);
        hasBubbleOn = true;
        spriteRenderer.sprite = withBubbleSprite;
        StopBlinking();
    }

    public void LooseBubble()
    {
        Debug.Log("Joueur perd sa bulle");
        hasBubbleOn = false;
        spriteRenderer.sprite = originalSprite;
        FindAnyObjectByType<FleurbulleManager>().resetFleurbulles();
    }

    private void ToggleSpriteColor()
    {
        spriteRenderer.color = (spriteRenderer.color == originalColor) ? blinkColor : originalColor;
        float remainingTime = suffocationCountdown - blinkTimer;
        float newBlinkInterval = initialBlinkInterval * (remainingTime / suffocationCountdown);
        CancelInvoke("ToggleSpriteColor");
        InvokeRepeating("ToggleSpriteColor", newBlinkInterval, newBlinkInterval);
    }

    private void StopBlinking()
    {
        isBlinking = false;
        CancelInvoke("ToggleSpriteColor");
        spriteRenderer.color = originalColor;
    }

    private void DieAndRespawn()
    {
        // Implement your respawn logic here
        Debug.Log("T'es mort");
        isDead = true;

        isDead = false;
        transform.SetPositionAndRotation(respawnPointPosition, respawnPointRotation);
    }

    private void Awake()
    {
        gBody = GetComponent<GravityBody>();

        originalColor = spriteRenderer.color;
        withBubbleSprite = Resources.Load<Sprite>("Player_with_bubble");
        originalSprite = spriteRenderer.sprite;

        transform.GetPositionAndRotation(out respawnPointPosition, out respawnPointRotation);
    }

    private void FixedUpdate()
    {
        if (!gBody.IsBreathable && !hasBubbleOn && !isDead)
        {
            if (!isBlinking)
            {
                isBlinking = true;
                blinkTimer = 0f;
                InvokeRepeating("ToggleSpriteColor", initialBlinkInterval, initialBlinkInterval);
            }

            blinkTimer += Time.deltaTime;
            Debug.Log("Jpeux pas respirer");

            if (blinkTimer >= suffocationCountdown)
            {
                StopBlinking();
                DieAndRespawn();
            }
        }
        else
        {
            StopBlinking();
        }
    }
}
