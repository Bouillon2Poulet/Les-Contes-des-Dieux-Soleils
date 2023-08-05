using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    float initialAngleX;
    public float speed = 1f;
    bool isClosing = true;
    public bool isUp;
    float maxAngle;
    float minAngle;

    // Start is called before the first frame update
    void Start()
    {
        maxAngle = (isUp)? 25f : 360f-25f;
        minAngle = (isUp)? 360f : 0;
    }

    // Update is called once per frame
    void Update()
    {
        int sens;
        if(isUp)
        {
            if(transform.localEulerAngles.x<minAngle && transform.localEulerAngles.x>minAngle-5f) isClosing = false;
            if(transform.localEulerAngles.x>maxAngle && transform.localEulerAngles.x<maxAngle+5f) isClosing = true;
            sens = (isClosing)? -1 : 1;
        }
        else
        {
            if(transform.localEulerAngles.x>minAngle && transform.localEulerAngles.x<minAngle+5f) isClosing = false;
            if(transform.localEulerAngles.x<maxAngle && transform.localEulerAngles.x>maxAngle-5f) isClosing = true;
            sens = (isClosing)? 1 : -1;
        }

        float rotationAmount = sens * speed * Time.deltaTime;
        transform.Rotate(rotationAmount, 0f, 0f);
        // Debug.Log(transform.localEulerAngles.x);
    }
}
