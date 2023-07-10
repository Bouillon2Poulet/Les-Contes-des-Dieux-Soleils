using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Status")]
    public bool isDead = false;
    public bool isAnimated = false;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private bool canSuffocate = true;

    [Header("Suffocation")]
    public bool hasBubbleOn = false;
    public float suffocationCountdown = 10f;
    public float initialBlinkInterval = 1f;
    public Color blinkColor = Color.red;

    private Color originalColor;
    private bool isBlinking;
    private float blinkTimer;

    private Sprite withBubbleSprite;
    private Sprite originalSprite;

    // Bubble Respawn
    private Vector3 respawnPointPosition;
    private Quaternion respawnPointRotation;

    [Header("Jump Respawn")]
    private bool hasJumpRespawnBeenInvoked;
    public float timeBeforeJumpRespawn = 22;

    private GravityBody gBody;

    public bool hasCosmoGuide = false;

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
        Debug.Log("T'es mort");
        isDead = true;
        // Va falloir animer une transition plus sympa ici
        isDead = false;
        transform.SetPositionAndRotation(respawnPointPosition, respawnPointRotation);
    }

    public void jumpRespawn()
    {
        transform.position = FindObjectOfType<ThirdPersonMovement>().lastJumpPosition;
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
        // Suffocation Logic (in GA)
        if (!gBody.IsBreathable && !hasBubbleOn && !isDead && canSuffocate && gBody.inGravityArea)
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

        // Void respawn (not in GA)
        if (!isAnimated && !gBody.inGravityArea && !hasJumpRespawnBeenInvoked)
        {
            Debug.Log("invoke JR");
            Invoke(nameof(jumpRespawn), timeBeforeJumpRespawn);
            hasJumpRespawnBeenInvoked = true;
        }

        if (hasJumpRespawnBeenInvoked && gBody.inGravityArea)
        {
            Debug.Log("cancel invoke JR");
            CancelInvoke(nameof(jumpRespawn));
            hasJumpRespawnBeenInvoked = false;
        }

        // Changement de Shader
        if (gBody.AreaShader != null)
        {
            spriteRenderer.material.shader = gBody.AreaShader;
        } else
        {
            spriteRenderer.material.shader = Shader.Find("Universal Render Pipeline/Lit");
        }
    }

    public void blockSuffocation()
    {
        canSuffocate = false;
    }
    public void unblockSuffocation()
    {
        canSuffocate = true;
    }
    public void stopAnimate()
    {
        isAnimated = false;
    }
    public void animate()
    {
        isAnimated = true;
    }
    public void hideSprite()
    {
        spriteRenderer.enabled = false;
    }
    public void showSprite()
    {
        spriteRenderer.enabled = true;
    }
}
