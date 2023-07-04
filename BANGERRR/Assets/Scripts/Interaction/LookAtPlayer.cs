using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject player;
    private Vector3 up;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        up = transform.up;
    }

    public void FixedUpdate()
    {
        transform.LookAt(player.transform.position, up);
    }

}
