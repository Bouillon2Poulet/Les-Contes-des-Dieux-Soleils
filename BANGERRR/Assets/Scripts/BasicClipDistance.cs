using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicClipDistance : MonoBehaviour
{
    [SerializeField] int clipDistance = 250;
    public Rigidbody player;
    public GameObject clippedObject;

    private bool wasVisible = true;

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(player.transform.position, clippedObject.transform.position);

        bool isVisible = distance < clipDistance;
        if (isVisible != wasVisible)
        {
            clippedObject.SetActive(isVisible);
            wasVisible = isVisible;
        }
    }
}
