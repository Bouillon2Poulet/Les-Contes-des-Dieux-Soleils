using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //the interaction rotation speed
    public float rotationSpeed = 8f;
    //the restrications of roation angle in y direction
    //0-1, 0 means 0-180 degree in y direction, 1 means can't roation in y direction.
    public float yRestriction = 0.5f;

    private float _pi = 3.1415926f;

    private void Update()
    {
        RotateCamera();
        ZoomCamera();
    }

    private void RotateCamera()
    {
        if (Input.GetMouseButton(0))
        {          
            float xRotation = rotationSpeed * Input.GetAxis("Mouse X");
            float yRotation = -rotationSpeed * Input.GetAxis("Mouse Y");

            //add the restrications of roation angle in y direction
            if (Math.Cos((transform.eulerAngles.x+yRotation) / 180f * _pi) < yRestriction)
                yRotation = 0;

            //Warning: the eulerAngles get from here may not the same with the value in inspector!
            //https://docs.unity3d.com/Manual/QuaternionAndEulerRotationsInUnity.html
            var angle = new Vector3(transform.eulerAngles.x + yRotation, transform.eulerAngles.y + xRotation, 0);
            
            transform.SetPositionAndRotation(transform.position,Quaternion.Euler(angle.x,angle.y,0));
        }
    }

    private void ZoomCamera()
    {
        float scrollFactor = Input.GetAxis("Mouse ScrollWheel");
        
        if (scrollFactor != 0)
        {
            transform.localScale = transform.localScale * (1f - scrollFactor);
        }
    }
}
