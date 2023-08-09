using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LineInstantiator.instance.hit = true;
        }
    }
}
