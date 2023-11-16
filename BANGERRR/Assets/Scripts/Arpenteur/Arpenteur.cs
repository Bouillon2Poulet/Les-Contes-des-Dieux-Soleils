using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Arpenteur : MonoBehaviour
{
    public static Arpenteur instance;

    public CinemachineFreeLook cam;
    public float cameraMoveSpeed = 5.0f;
    public float cameraRotationSpeed = 50.0f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        cam = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        MoveCamera();
        RotateCamera();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            cameraMoveSpeed /= 10;
            cameraRotationSpeed /= 5;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            cameraMoveSpeed *= 10;
            cameraRotationSpeed *= 5;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            cameraMoveSpeed /= 100;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            cameraMoveSpeed *= 100;
        }
    }

    void MoveCamera()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.position += cam.transform.TransformDirection(Vector3.forward) * cameraMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            cam.transform.position -= cam.transform.TransformDirection(Vector3.forward) * cameraMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.position += cam.transform.TransformDirection(Vector3.left) * cameraMoveSpeed * Time.deltaTime; 
        }
        if (Input.GetKey(KeyCode.D))
        {
            cam.transform.position -= cam.transform.TransformDirection(Vector3.left) * cameraMoveSpeed * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.Space))
        {
            cam.transform.position += cam.transform.TransformDirection(Vector3.up) * cameraMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            cam.transform.position -= cam.transform.TransformDirection(Vector3.up) * cameraMoveSpeed * Time.deltaTime;
        }
    }

    void RotateCamera()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            cam.transform.Rotate(Vector3.up, cameraRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            cam.transform.Rotate(Vector3.up, -cameraRotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            cam.transform.Rotate(Vector3.left, cameraRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            cam.transform.Rotate(Vector3.left, -cameraRotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            cam.transform.Rotate(Vector3.forward, cameraRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            cam.transform.Rotate(Vector3.forward, -cameraRotationSpeed * Time.deltaTime);
        }
    }
}
