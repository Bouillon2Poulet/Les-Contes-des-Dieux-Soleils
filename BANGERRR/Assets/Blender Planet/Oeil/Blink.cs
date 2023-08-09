using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public float speed = 1f;
    bool isClosing = false;
    public bool isUp;
    float maxAngle;
    float minAngle;
    int sens;

    private bool isBlinking = false;
    private int nextYouStop = 0;
    void Start()
    {
        maxAngle = (isUp) ? 25f : 360f-25f;
        minAngle = (isUp) ? 360f : 0;
    }

    public void Trigger(int amount)
    {
        if (!isBlinking)
        {
            isBlinking = true;
            nextYouStop = amount;
        }
    }

    void Update()
    {
        if (isBlinking)
        {
            if (isUp)
            {
                if (transform.localEulerAngles.x > maxAngle && transform.localEulerAngles.x < maxAngle + 5f)
                {
                    if (nextYouStop <= 0)
                    {
                        isBlinking = false;
                    }
                    isClosing = true;
                }
                if (transform.localEulerAngles.x < minAngle && transform.localEulerAngles.x > minAngle - 5f)
                {
                    isClosing = false;
                    nextYouStop -= 1;
                }
                sens = (isClosing) ? -1 : 1;
            }
            else
            {
                if (transform.localEulerAngles.x < maxAngle && transform.localEulerAngles.x > maxAngle - 5f)
                {
                    if (nextYouStop <= 0)
                    {
                        isBlinking = false;
                    }
                    isClosing = true;
                }
                if (transform.localEulerAngles.x > minAngle && transform.localEulerAngles.x < minAngle + 5f) 
                {
                    isClosing = false;
                    nextYouStop -= 1;
                }
                sens = (isClosing) ? 1 : -1;
            }

            float rotationAmount = sens * speed * Time.deltaTime;
            transform.Rotate(rotationAmount, 0f, 0f);
            //Debug.Log(isClosing);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("touché !!");
            nextYouStop = 0;
            PlayerStatus.instance.HitBlink();
            PhaseManager.instance.TriggerReset();
        }
    }

    public bool IsBlinking()
    {
        return isBlinking;
    }
}
