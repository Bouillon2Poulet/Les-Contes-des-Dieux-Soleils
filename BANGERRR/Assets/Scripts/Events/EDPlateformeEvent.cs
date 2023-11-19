using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDPlateformeEvent : MonoBehaviour
{
    public Transform platform;
    public Transform start;
    public Transform end;
    public float speed = .002f;

    private float platformTime = 0f;
    private bool isAscending = false;
    private bool isDescending = false;
    private bool atTop = false;
    private bool atBottom = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!isAscending && !isDescending && !atTop && atBottom)
        {
            isAscending = true;
            AudioManager.instance.FadeIn("concreteloop", 20);
            atBottom = false;
            platformTime = 0f;
            FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isAscending && !isDescending && !atBottom && atTop)
        {
            isDescending = true;
            AudioManager.instance.Play("concreteloop");
            AudioManager.instance.FadeOut("concreteloop", 120);
            atTop = false;
            platformTime = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (isAscending)
        {
            platformTime += speed;
            platform.position = Vector3.Lerp(start.position, end.position, platformTime);
            if (platformTime >= 1)
            {
                isAscending = false;
                AudioManager.instance.Stop("concreteloop");
                atTop = true;
                FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            }
        }
        if (isDescending)
        {
            platformTime += speed;
            platform.position = Vector3.Lerp(end.position, start.position, platformTime);
            if (platformTime >= 1)
            {
                isDescending = false;
                AudioManager.instance.Stop("concreteloop");
                atBottom = true;
            }
        }
    }
}
