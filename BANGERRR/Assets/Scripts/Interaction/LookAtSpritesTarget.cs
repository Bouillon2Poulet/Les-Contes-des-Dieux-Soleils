using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSpritesTarget : MonoBehaviour
{
    private GameObject player;
    private Vector3 up;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("SpritesTarget");
        up = transform.up;
    }

    public void FixedUpdate()
    {
        transform.LookAt(player.transform.position, up);
    }

}
