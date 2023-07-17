using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isSolisedeAlignedWithSolimont : MonoBehaviour
{
    [SerializeField] private Transform holeDirectionPoint;
    [SerializeField] private Transform solisede;
    [SerializeField] private float threshold = 10f;

    public bool Check()
    {
        Vector3 solimontPos = transform.position;
        Vector3 holeVector = holeDirectionPoint.position - solimontPos;
        Vector3 solimontToSolisede = solisede.position - solimontPos;
        float angle = Vector3.Angle(holeVector, solimontToSolisede);
        return (angle < threshold) ? true : false;
    }
}
