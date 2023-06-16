using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deugCamVectorIndicator : MonoBehaviour
{
    public GameObject camObject;
    private float rotationSpeed = 5;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 forwardDirection = camObject.transform.forward;
        Vector3 upDirection = camObject.transform.up;
        //Vector3 rightDirection = camObject.transform.right;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(forwardDirection, upDirection), upDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
