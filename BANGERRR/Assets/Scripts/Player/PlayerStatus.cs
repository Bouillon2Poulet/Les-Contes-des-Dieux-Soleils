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

    [Header("Bubble Respawn")]
    public Transform bubbleRespawnTransform;
    public GameObject Bulle;
    public Sprite BulleSprite;
    public Sprite BulleGoing1;
    public Sprite BulleGoing2;
    public Sprite BulleGoing3;
    public Sprite BulleEmpty;
    //private Vector3 respawnPointPosition;
    //private Quaternion respawnPointRotation;

    [Header("Solimont")]
    public GameObject rockOnHead;

    [Header("Jump Respawn")]
    public float timeBeforeJumpRespawn = 22;
    private bool hasJumpRespawnBeenInvoked;

    private GravityBody gBody;

    [Header("Cosmoguide")]
    public bool hasCosmoGuide = false;

    public void WearBubble()
    {
        Debug.Log("Joueur porte une bulle");
        //transform.GetPositionAndRotation(out respawnPointPosition, out respawnPointRotation);
        bubbleRespawnTransform.SetPositionAndRotation(transform.position, transform.rotation);
        Bulle.GetComponent<SpriteRenderer>().sprite = BulleSprite;
        hasBubbleOn = true;
        StopBlinking();
    }

    public void LooseBubble()
    {
        Debug.Log("Joueur perd sa bulle");
        StartCoroutine(nameof(BulleGoingAnimation));
        hasBubbleOn = false;
        FindAnyObjectByType<FleurbulleManager>().resetFleurbulles();
    }

    IEnumerator BulleGoingAnimation()
    {
        Bulle.GetComponent<SpriteRenderer>().sprite = BulleGoing1;
        yield return new WaitForSeconds(.05f);
        Bulle.GetComponent<SpriteRenderer>().sprite = BulleGoing2;
        yield return new WaitForSeconds(.05f);
        Bulle.GetComponent<SpriteRenderer>().sprite = BulleGoing3;
        yield return new WaitForSeconds(.05f);
        Bulle.GetComponent<SpriteRenderer>().sprite = BulleEmpty;
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

    public void HitBlink()
    {
        StartCoroutine(HitBlinker());
    }

    private IEnumerator HitBlinker()
    {
        spriteRenderer.color = blinkColor;
        yield return new WaitForSeconds(.5f);
        spriteRenderer.color = originalColor;
        Debug.Log("Hitblinked");
    }

    private void DieAndRespawn()
    {
        Debug.Log("T'es mort");
        isDead = true;
        // Va falloir animer une transition plus sympa ici
        isDead = false;
        //transform.SetPositionAndRotation(respawnPointPosition, respawnPointRotation);
        transform.SetPositionAndRotation(bubbleRespawnTransform.position, bubbleRespawnTransform.rotation);
    }

    public void JumpRespawn()
    {
        GetComponent<Rigidbody>().position = LastJumpPosition.instance.transform.position;
    }
    public static PlayerStatus instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        gBody = GetComponent<GravityBody>();

        originalColor = spriteRenderer.color;
    }

    private void FixedUpdate()
    {
        // Changement de Shader
        if (gBody.AreaShader != null)
        {
            spriteRenderer.material.shader = gBody.AreaShader;
        }
        else
        {
            spriteRenderer.material.shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
        }

        // Suffocation Logic (in GA)
        //if (true)
        if (!gBody.IsBreathable && !hasBubbleOn && !isDead && canSuffocate && gBody.inGravityArea)
            {
            if (!isBlinking)
            {
                isBlinking = true;
                blinkTimer = 0f;
                InvokeRepeating("ToggleSpriteColor", initialBlinkInterval, initialBlinkInterval);
            }

            blinkTimer += Time.deltaTime;
            //Debug.Log("Jpeux pas respirer");

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
            //Debug.Log("invoke JR");
            Invoke(nameof(JumpRespawn), timeBeforeJumpRespawn);
            hasJumpRespawnBeenInvoked = true;
        }

        if (hasJumpRespawnBeenInvoked && gBody.inGravityArea)
        {
            //Debug.Log("cancel invoke JR");
            CancelInvoke(nameof(JumpRespawn));
            hasJumpRespawnBeenInvoked = false;
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
        if (hasBubbleOn)
        {
            Bulle.SetActive(false);
        }
    }
    public void showSprite()
    {
        spriteRenderer.enabled = true;
        if (hasBubbleOn)
        {
            Bulle.SetActive(true);
        }
    }
    public void giveCosmoguide()
    {
        hasCosmoGuide = true;
        KeyInteractionManager.instance.ActivateCosmoguideIcon();
    }

    public void PutRockOnHead()
    {
        rockOnHead.SetActive(true);
    }

    public void RemoveRockOnHead()
    {
        rockOnHead.SetActive(false);
    }

    public void GameMenuCursor(bool inMenu)
    {
        if (inMenu)
        {
            Debug.Log("IN game menu");
            FindObjectOfType<ThirdPersonMovement>().blockPlayerMoveInputs();
            FindObjectOfType<MainCameraManager>().blockMovement();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.Log("OUT game menu");
            if (!DialogManager.instance.isItActive())
            {
                FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            }
            FindObjectOfType<MainCameraManager>().unblockMovement();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
