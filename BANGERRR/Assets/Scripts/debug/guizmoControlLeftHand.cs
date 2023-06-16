using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guizmoControlLeftHand : MonoBehaviour
{
    public Transform gravity;

    public float rotationSpeed = 5f;
    private float cameraRotationOffsetX = 0f;
    private float cameraRotationOffsetY = 0f;

    Quaternion moveX = Quaternion.Euler(1f, 0, 0);
    Quaternion moveMX = Quaternion.Euler(-1f, 0, 0);
    Quaternion moveY = Quaternion.Euler(0, 1f, 0);
    Quaternion moveMY = Quaternion.Euler(0, -1f, 0);
    Quaternion moveZ = Quaternion.Euler(0, 0, 1f);
    Quaternion moveMZ = Quaternion.Euler(0, 0, -1f);

    void FixedUpdate()
    {
        Vector3 gravityDirection = gravity.up;
        Quaternion camRotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;
        // Apply rotation offsets for camera
        Quaternion offsetRotation = Quaternion.Euler(cameraRotationOffsetX, cameraRotationOffsetY, 0f);
        Quaternion finalRotation = camRotation * offsetRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, rotationSpeed * Time.deltaTime);

        /// x offset
        if (Input.GetKey(KeyCode.R))
        {
            cameraRotationOffsetX += 1f;
        }
        if (Input.GetKey(KeyCode.F))
        {
            cameraRotationOffsetX -= 1f;
        }

        /// y offset
        if (Input.GetKey(KeyCode.C))
        {
            cameraRotationOffsetY += 1f;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            cameraRotationOffsetY -= 1f;
        }

        /// transform
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation *= moveX;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation *= moveMX;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation *= moveY;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation *= moveMY;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.rotation *= moveZ;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.rotation *= moveMZ;
        }
    }
}
