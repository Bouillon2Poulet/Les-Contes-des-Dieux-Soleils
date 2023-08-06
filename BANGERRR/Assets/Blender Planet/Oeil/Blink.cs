using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    float initialAngleX;
    public float speed = 1f;
    bool isClosing = false;
    public bool isUp;
    float maxAngle;
    float minAngle;
    int sens;

    private bool isBlinking = false;
    private bool nextYouStop = false;
    void Start()
    {
        maxAngle = (isUp)? 25f : 360f-25f;
        minAngle = (isUp)? 360f : 0;
    }

    public void OneTime()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            nextYouStop = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OneTime();
        }

        if (isBlinking)
        {
            if (isUp)
            {
                if (transform.localEulerAngles.x > maxAngle && transform.localEulerAngles.x < maxAngle + 5f)
                {
                    if (nextYouStop)
                    {
                        isBlinking = false;
                    }
                    isClosing = true;
                }
                if (transform.localEulerAngles.x < minAngle && transform.localEulerAngles.x > minAngle - 5f)
                {
                    isClosing = false;
                    nextYouStop = true;
                }
                sens = (isClosing) ? -1 : 1;
            }
            else
            {
                if (transform.localEulerAngles.x < maxAngle && transform.localEulerAngles.x > maxAngle - 5f)
                {
                    if (nextYouStop)
                    {
                        isBlinking = false;
                    }
                    isClosing = true;
                }
                if (transform.localEulerAngles.x > minAngle && transform.localEulerAngles.x < minAngle + 5f) 
                {
                    isClosing = false;
                    nextYouStop = true;
                }
                sens = (isClosing) ? 1 : -1;
            }

            float rotationAmount = sens * speed * Time.deltaTime;
            transform.Rotate(rotationAmount, 0f, 0f);
            //Debug.Log(isClosing);
        }
    }
}
