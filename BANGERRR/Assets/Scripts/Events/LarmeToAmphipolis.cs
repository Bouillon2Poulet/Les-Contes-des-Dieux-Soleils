using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarmeToAmphipolis : MonoBehaviour
{
    public Transform LAC1;
    public Transform LAS2;
    public Transform LAS3;
    //public Transform LAC4; // Should be center between 3 and 5
    private Vector3 Midpoint;
    public Transform LAT5;
    public Transform LAT6;
    public Transform LAA7;
    public Transform larmeTarget;

    [SerializeField] private float animationSpeed;
    private float interpolateAmount;
    private float animationProgress;
    private bool hasAnimationStarted = false;
    private bool hasAnimationStopped = false;
    Quaternion rotationOffset = Quaternion.Euler(180f, 0f, 0f);

    private bool isPlayerOnboard = false;
    private bool larmeHasDefinitlyLanded = false;
    public Transform futureParentOfLarme;
    public GameObject LarmeGA;

    // debug
    bool debugCourbe = false;
    bool hasSpheresBeenCreated = false;
    List<GameObject> spheresList;
    bool hasPositionsOnCurveBeenLogged = false;

    private void FixedUpdate()
    {
        if (FindAnyObjectByType<SystemDayCounter>().hour == 1 && !hasAnimationStarted && !hasAnimationStopped && !larmeHasDefinitlyLanded) // BEGIN ANIMATION
        {
            hasAnimationStarted = true;
        }
        else if (hasAnimationStarted && !hasAnimationStopped) // DO ANIMATION
        {
            Midpoint = (LAS3.position + LAT5.position) / 2;

            // The debug of the courbe ^^ :
            if (debugCourbe)
            {
                if (!hasSpheresBeenCreated)
                {
                    // Spheres creation
                    spheresList = new List<GameObject>();
                    for (int i = 0; i < 500; i++)
                    {
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.localScale = new Vector3(3.2f, 3.2f, 3.2f);
                        spheresList.Add(sphere);
                    }
                    hasSpheresBeenCreated = true;

                    // midpoint cude 
                    GameObject midpointCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    midpointCube.transform.position = Midpoint;

                    // Pause system
                    FindAnyObjectByType<SystemDayCounter>().pauseSystem();
                }
                if (hasSpheresBeenCreated)
                {
                    // Spheres positon update
                    for (int i = 0; i < spheresList.Count; i++)
                    {
                        float index = i;
                        float total = spheresList.Count;
                        float positionOnCurve = (index / total) * 2;
                        if (positionOnCurve < 1)
                            spheresList[i].transform.position = CubicLerp(LAC1.position, LAS2.position, LAS3.position, Midpoint, positionOnCurve % 1f);
                        else
                            spheresList[i].transform.position = CubicLerp(Midpoint, LAT5.position, LAT6.position, LAA7.position, positionOnCurve % 1f);
                        if (!hasPositionsOnCurveBeenLogged) Debug.Log(positionOnCurve);
                    }
                    if (!hasPositionsOnCurveBeenLogged) hasPositionsOnCurveBeenLogged = true;
                }
            }

            // The animation :
            if (interpolateAmount > 1)
            {
                transform.position = CubicLerp(Midpoint, LAT5.position, LAT6.position, LAA7.position, animationProgress);
                larmeTarget.position = CubicLerp(Midpoint, LAT5.position, LAT6.position, LAA7.position, animationProgress + .001f);
            }
            else
            {
                transform.position = CubicLerp(LAC1.position, LAS2.position, LAS3.position, Midpoint, animationProgress);
                larmeTarget.position = CubicLerp(LAC1.position, LAS2.position, LAS3.position, Midpoint, animationProgress + .001f);
            }
            interpolateAmount += Time.fixedDeltaTime * animationSpeed;
            animationProgress = interpolateAmount % 1f;

            transform.LookAt(larmeTarget.position);
            transform.rotation *= rotationOffset;

            if (interpolateAmount > 2)
            {
                hasAnimationStopped = true;
                if (isPlayerOnboard)
                {
                    transform.SetParent(futureParentOfLarme);
                    larmeHasDefinitlyLanded = true;
                    Debug.Log("Larme has landed for good");
                } 
                else
                {
                    transform.position = new Vector3(5000f, 5000f, 5000f);
                }
            }
        }
        else if (FindAnyObjectByType<SystemDayCounter>().hour == 17 && !larmeHasDefinitlyLanded) // RESET ANIMATION
        {
            interpolateAmount = 0f;
            hasAnimationStarted = false;
            hasAnimationStopped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnboard = true;
            Debug.Log("Player is on board!");
        }
    }

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
}
