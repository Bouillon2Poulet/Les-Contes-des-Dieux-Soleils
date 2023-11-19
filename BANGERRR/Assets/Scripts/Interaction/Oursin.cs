using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oursin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStatus player) && player.hasBubbleOn)
        {
            player.LooseBubble();
            AudioManager.instance.Play("bullepete");
        }
    }
}
