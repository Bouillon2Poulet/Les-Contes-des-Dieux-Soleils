using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guizmoControlRightHand : MonoBehaviour
{
    Quaternion moveX = Quaternion.Euler(1f, 0, 0);
    Quaternion moveMX = Quaternion.Euler(-1f, 0, 0);
    Quaternion moveY = Quaternion.Euler(0, 1f, 0);
    Quaternion moveMY = Quaternion.Euler(0, -1f, 0);
    Quaternion moveZ = Quaternion.Euler(0, 0, 1f);
    Quaternion moveMZ = Quaternion.Euler(0, 0, -1f);

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.rotation *= moveX;
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            transform.rotation *= moveMX;
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.rotation *= moveY;
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.rotation *= moveMY;
        }
        if (Input.GetKey(KeyCode.Keypad7))
        {
            transform.rotation *= moveZ;
        }
        if (Input.GetKey(KeyCode.Keypad9))
        {
            transform.rotation *= moveMZ;
        }
    }
}
