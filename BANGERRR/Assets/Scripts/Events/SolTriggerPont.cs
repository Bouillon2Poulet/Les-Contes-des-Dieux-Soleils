using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolTriggerPont : MonoBehaviour
{
    private Vector3 startPos;
    public Transform pointB;
    public Transform pointC;
    public Transform landingPoint;
    public Rigidbody playerRB;

    private bool isPlayerIn = false;
    private bool hasJumped = false;

    [Header("Animation Control")]
    [SerializeField] private float animationSpeed;
    private float interpolateAmount;
    private bool hasAnimationStarted = false;
    private bool hasAnimationStopped = false;

    private void FixedUpdate()
    {
        if (isPlayerIn && !hasJumped)
        {
            StartSolAnimation();
            AudioManager.instance.FadeOut("rituel", 50);
            hasJumped = true;
        }
        if (!hasAnimationStopped)
        {
            if (hasAnimationStarted)
            {
                float animationDuration = 10;
                animationSpeed = 2f / animationDuration;
                float t = Mathf.Clamp01(interpolateAmount);
                float interpolatedT = CubicEaseInOut(t);

                playerRB.MovePosition(CubicLerp(startPos, pointB.position, pointC.position, landingPoint.position, interpolatedT));
                
                interpolateAmount += Time.fixedDeltaTime * animationSpeed;
            }
            if (interpolateAmount >= 1)
            {
                StopAnimation();
                SystemDayCounter.instance.resumeSystem();
                FindAnyObjectByType<SolRituelStarter>().LeVraiPont.SetActive(false);
            }
        }
    }

    private void StartSolAnimation()
    {
        startPos = playerRB.position;
        FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
        FindAnyObjectByType<ThirdPersonMovement>().blockPlayerGAFollow();
        FindAnyObjectByType<ThirdPersonMovement>().UncapSpeed();
        FindAnyObjectByType<PlayerStatus>().blockSuffocation();
        FindAnyObjectByType<PlayerStatus>().animate();
        hasAnimationStarted = true;
    }

    private void StopAnimation()
    {
        FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerGAFollow();
        FindAnyObjectByType<ThirdPersonMovement>().CapSpeed();
        FindAnyObjectByType<PlayerStatus>().unblockSuffocation();
        FindAnyObjectByType<PlayerStatus>().stopAnimate();
        hasAnimationStopped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerIn = true;
            Debug.Log("Saut du pont !");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerIn = false;
        }
    }

    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, interpolateAmount);
    }

    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);
        return Vector3.Lerp(ab_bc, bc_cd, interpolateAmount);
    }

    float CubicEaseInOut(float t)
    {
        t = Mathf.Clamp01(t);
        return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }
}
