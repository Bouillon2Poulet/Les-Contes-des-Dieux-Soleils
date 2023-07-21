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

    [SerializeField] private float animationSpeed;
    private float interpolateAmount;
    private float animationProgress;
    private bool hasAnimationStarted = false;
    private bool hasAnimationStopped = false;


    private void FixedUpdate()
    {
        if (FindAnyObjectByType<SystemDayCounter>().hour == 12 && !hasAnimationStarted)
        {
            hasAnimationStarted = true;
        }
        if (hasAnimationStarted)
        {
            Midpoint = (LAS3.position + LAT5.position) / 2;

            if (interpolateAmount > 1)
            {
                transform.position = CubicLerp(Midpoint, LAT5.position, LAT6.position, LAA7.position, animationProgress);
            }
            else
            {
                transform.position = CubicLerp(LAC1.position, LAS2.position, LAS3.position, Midpoint, animationProgress);
            }
            interpolateAmount += Time.fixedDeltaTime * animationSpeed;
            animationProgress = interpolateAmount % 1f;
            if (interpolateAmount > 2)
            {
                hasAnimationStarted = false;
                hasAnimationStopped = true;
            }
        }
        if (hasAnimationStopped)
        {
            interpolateAmount = 0f;
            hasAnimationStopped = false;
        }
        Debug.Log(interpolateAmount);
    }



    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, animationProgress);
    }

    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);
        return Vector3.Lerp(ab_bc, bc_cd, animationProgress);
    }

    float CubicEaseInOut(float t)
    {
        t = Mathf.Clamp01(t);
        return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }
}
