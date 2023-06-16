using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugOnPlayerGravityIndicator : MonoBehaviour
{
    public GameObject playerObject;
    //private GravityBody gravityBody;

    private float rotationSpeed = 5;

    void Start()
    {
        //gravityBody = playerObject.GetComponent<GravityBody>();
    }

    void Update()
    {
        Vector3 forwardDirection = playerObject.transform.forward;
        Vector3 upDirection = playerObject.transform.up;
        //Vector3 rightDirection = camObject.transform.right;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(forwardDirection, upDirection), upDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
