using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpSwimNinjaEvents : MonoBehaviour
{
    public GameObject bubble;

    [Header("General")]
    public Rigidbody playerRb;
    public Transform NinjaSprite;
    private Transform SwimNinjaT;
    private ThirdPersonMovement playerMovement;
    private bool full = false;
    private bool firstTime = false;

    [Header("Mount/Dismount")]
    public Transform animPointMounted;
    public Transform animPointUp1;
    public Transform animPointUp2;
    public Transform animPointAside;
    public Transform playerPos;
    public bool isPlayerMounting = false;
    private float mountAnimProgress;
    private float mountAnimSpeed = .5f;
    public bool isPlayerDismounting = false;
    private float dismountAnimProgress;
    private float dismountAnimSpeed = .5f;

    [Header("Ninja Move")]
    public Transform animPointNinjaOrigin;
    public Transform animPointNinja1;
    public Transform animPointNinja2;
    public Transform animPointNinjaEnd;
    public bool isNinjaAscending = false;
    public bool isNinjaDescending = false;
    private float NinjaProgression;
    private float NinjaSpeed = .25f;

    public PointingTowards amphipolisRotationScript;

    private void Start()
    {
        playerMovement = FindObjectOfType<ThirdPersonMovement>();
        SwimNinjaT = transform;
    }

    public void MountPlayer()
    {
        amphipolisRotationScript.pointingTowards = null;
        ToggleBubble(false);
        playerMovement.blockPlayerMoveInputs();
        playerMovement.blockPlayerGAFollow();
        playerMovement.UncapSpeed();
        playerPos.position = playerRb.position;
        mountAnimProgress = 0;
        isPlayerMounting = true;
    }

    public void MountPlayerFull()
    {
        full = true;
        MountPlayer();
    }

    public void MountPlayerFirstTime()
    {
        firstTime = true;
        MountPlayer();
    }

    public void DismountPlayer()
    {
        playerRb.gameObject.transform.SetParent(null);
        dismountAnimProgress = 0;
        isPlayerDismounting = true;
    }

    public void AscendNinja()
    {
        NinjaProgression = 0;
        isNinjaAscending = true;
        SwimNinjaT.gameObject.GetComponent<GravityBody>().SetForceApplication(false);
    }
    public void DescendNinja()
    {
        NinjaProgression = 0;
        isNinjaDescending = true;
    }

    private void FixedUpdate()
    {
        if (isPlayerMounting)
        {
            playerRb.MovePosition(Animation(playerPos.position, animPointUp2.position, animPointUp1.position, animPointMounted.position, mountAnimProgress));
            mountAnimProgress += Time.fixedDeltaTime * mountAnimSpeed;
            if (mountAnimProgress >= 1)
            {
                isPlayerMounting = false;
                playerRb.gameObject.transform.SetParent(SwimNinjaT);
                Debug.Log("Mounting complete");
                Invoke(nameof(AscendNinja), 1);
            }
        }
        else if (isPlayerDismounting)
        {
            playerRb.MovePosition(Animation(animPointMounted.position, animPointUp1.position, animPointUp2.position, animPointAside.position, dismountAnimProgress));
            dismountAnimProgress += Time.fixedDeltaTime * dismountAnimSpeed;
            if (dismountAnimProgress >= 1)
            {
                isPlayerDismounting = false;
                FindObjectOfType<NPCSwimNinja>().ShowBubble();
                playerMovement.unblockPlayerMoveInputs();
                playerMovement.unblockPlayerGAFollow();
                playerMovement.CapSpeed();
                Debug.Log("Dismounting complete");
                Invoke(nameof(DescendNinja), 1);
            }
        }

        if (isNinjaAscending)
        {
            SwimNinjaT.position = Animation(animPointNinjaOrigin.position, animPointNinja1.position, animPointNinja2.position, animPointNinjaEnd.position, NinjaProgression);
            NinjaProgression += Time.fixedDeltaTime * NinjaSpeed;
            if (NinjaProgression >= 1)
            {
                isNinjaAscending = false;
                Debug.Log("Ninja Ascended");
                if (full)
                {
                    Invoke(nameof(DismountPlayer), 1);
                }
                else if (firstTime)
                {
                    FindObjectOfType<NPCSwimNinja>().DoPageD();
                }
            }
        }
        else if (isNinjaDescending)
        {
            SwimNinjaT.position = Animation(animPointNinjaEnd.position, animPointNinja2.position, animPointNinja1.position, animPointNinjaOrigin.position, NinjaProgression);
            NinjaProgression += Time.fixedDeltaTime * NinjaSpeed;
            if (NinjaProgression >= 1)
            {
                isNinjaDescending = false;
                ToggleBubble(true);
                SwimNinjaT.gameObject.GetComponent<GravityBody>().SetForceApplication(true);
                full = false;
                FindObjectOfType<NPCSwimNinja>().isInteractionAllowed = true;
                Debug.Log("Ninja Descended");
            }
        }

        if (isFlipping)
        {
            angle += Time.fixedDeltaTime * 100f;
            NinjaSprite.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            if (angle >= 180f)
            {
                isFlipping = false;
            }
        }
    }

    /*public void ForceMountPosition()
    {
        playerRb.position = animPointMounted.position;
    }*/

    private float angle = 0f;
    private bool isFlipping = false;

    public void FlipNinja()
    {
        isFlipping = true;
    }

    private void ToggleBubble(bool state)
    {
        bubble.SetActive(state);
    }


    // Animation functions
    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }

    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);
        return Vector3.Lerp(ab_bc, bc_cd, t);
    }

    float CubicEaseInOut(float t)
    {
        t = Mathf.Clamp01(t);
        return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }

    private Vector3 Animation(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float p)
    {
        float interpolatedP = CubicEaseInOut(p);
        return CubicLerp(a, b, c, d, interpolatedP);
    }

    // Singleton
    public static AmpSwimNinjaEvents instance { get; private set; }
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
    }
}
