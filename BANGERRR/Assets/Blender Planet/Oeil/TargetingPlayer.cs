using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingPlayer : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        transform.LookAt(player);
    }
}
