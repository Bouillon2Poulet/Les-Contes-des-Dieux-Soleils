using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guizmoPlayerOrientation : MonoBehaviour
{
    public Transform gravity;
    public Transform cam;

    public float rotationSpeed = 5f;
    private float playerRotationOffset = 0f;


    void FixedUpdate()
    {
        Vector3 gravityDirection = gravity.up;
        Vector3 forwardDirection = cam.forward;

        // Set the player's rotation to match the gravity direction on the y-axis only
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(forwardDirection, gravityDirection), gravityDirection) * Quaternion.Euler(0f, playerRotationOffset, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRotationOffset += 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRotationOffset -= 1f;
        }
    }
}
