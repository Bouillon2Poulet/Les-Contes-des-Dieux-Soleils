using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugPlanetRotation : MonoBehaviour
{
    public Transform centerPoint; // The point around which the planet will rotate
    public float rotationSpeed = 1.0f; // The speed of rotation
    public float distance = 100.0f; // The distance of the planet from the center point
    public float selfRotationSpeed = 100;


    void FixedUpdate()
    {
        // Calculate the angle of rotation based on the elapsed time and rotation speed
        float angle = Time.time * rotationSpeed;

        // Calculate the position of the planet along a circle using polar coordinates
        float x = centerPoint.position.x + Mathf.Cos(angle) * distance;
        float z = centerPoint.position.z + Mathf.Sin(angle) * distance;

        // Update the planet's transform position
        transform.position = new Vector3(x, transform.position.y, z);

        // Add slow rotation to the planet itself
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * selfRotationSpeed);
    }
}
