using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing_spline : MonoBehaviour
{
    [SerializeField] private float interpolateAmont;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Transform B;
    [SerializeField] private Transform C;
    [SerializeField] private Transform D;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.position = CubicLerp(startPos, B.position, C.position, D.position, interpolateAmont);
    }

    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, interpolateAmont);
    }

    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);
        return Vector3.Lerp(ab_bc, bc_cd, interpolateAmont);
    }
}
