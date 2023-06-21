using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    //Attach this script to the camera//

    public GameObject player;
    public GameObject mainCamera;

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, mainCamera.transform.forward, 500.0F);
        int i = 0;
        foreach (RaycastHit hit in hits)
        {
            Debug.Log(i);
            i++;
            if(hit.distance > dist)
            {
                Debug.Log("!!");
                hit.transform.gameObject.layer = LayerMask.NameToLayer("BackGround");
            }
            if (hit.distance < dist)
            {
                hit.transform.gameObject.layer = LayerMask.NameToLayer("ForeGround");
            }
            // if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            // {
            //     continue;
            // }
        }
    }
}
