using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTP : MonoBehaviour
{
    public Rigidbody player;
    public KeyCode TPkey;

    private void teleport()
    {
        player.position = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(TPkey))
        {
            teleport();
        }
    }
}
