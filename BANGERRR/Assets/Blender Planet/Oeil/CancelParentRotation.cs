using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelParentRotation : MonoBehaviour
{
    public Transform child;

    void FixedUpdate()
    {
        float x = gameObject.transform.rotation.x * -1.0f;
        float y = gameObject.transform.rotation.y * -1.0f;
        float z = gameObject.transform.rotation.z * -1.0f;

        child.transform.rotation = Quaternion.Euler(x, y, z);
    }
}
