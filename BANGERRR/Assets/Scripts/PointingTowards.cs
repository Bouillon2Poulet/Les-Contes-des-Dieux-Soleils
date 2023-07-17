using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingTowards : MonoBehaviour
{
    [Header("Pointing Towards")]
    public Transform pointingTowards;
    public Quaternion rotationOffset;

    private void FixedUpdate()
    {
        if (pointingTowards != null)
        {
            transform.LookAt(pointingTowards.position);
            transform.rotation *= rotationOffset;
        }
    }
}
