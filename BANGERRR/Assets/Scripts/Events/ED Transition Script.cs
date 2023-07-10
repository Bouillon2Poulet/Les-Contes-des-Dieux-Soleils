using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDTransitionScript : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] public GameObject player;
    private Rigidbody playerRB;

    [Header("Animation Points")]
    private Vector3 startPos;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private Transform landingPoint;

    [Header("Animation Control")]
    [SerializeField] private float animationSpeed;
    private float interpolateAmount;
    [SerializeField] private bool hasAnimationStarted = false;
    [SerializeField] private bool hasAnimationStopped = false;

    private void Awake()
    {
        playerRB = player.GetComponent<Rigidbody>();
    }

    private void Start() // DEBUG - TO DELETE
    {
        Invoke("startAnimation", 2);
    }

    public void startAnimation()
    {
        startPos = player.transform.position;
        FindObjectOfType<ThirdPersonMovement>().blockPlayerMoveInputs();
        FindObjectOfType<ThirdPersonMovement>().blockPlayerGAFollow();
        FindObjectOfType<PlayerStatus>().blockSuffocation();
        FindObjectOfType<PlayerStatus>().animate();
        hasAnimationStarted = true;
    }

    private void stopAnimation()
    {
        FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        FindObjectOfType<ThirdPersonMovement>().unblockPlayerGAFollow();
        FindObjectOfType<PlayerStatus>().unblockSuffocation();
        FindObjectOfType<PlayerStatus>().stopAnimate();
        hasAnimationStopped = true;
    }

    private void FixedUpdate()
    {
        if (!hasAnimationStopped)
        {
            if (hasAnimationStarted)
            {
                // Déclarez une variable pour la durée totale de l'animation
                float animationDuration = 100;

                // Calculez la vitesse de l'animation en fonction de la durée
                animationSpeed = 2f / animationDuration;

                // Calculez la valeur de "interpolateAmount" en utilisant une fonction d'interpolation personnalisée
                float t = Mathf.Clamp01(interpolateAmount); // Assurez-vous que t est compris entre 0 et 1
                float interpolatedT = CubicEaseInOut(t);

                playerRB.MovePosition(CubicLerp(startPos, pointB.position, pointC.position, landingPoint.position, interpolatedT));

                interpolateAmount += Time.fixedDeltaTime * animationSpeed;
            }
            if (interpolateAmount >= 1)
            {
                stopAnimation();
            }
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
