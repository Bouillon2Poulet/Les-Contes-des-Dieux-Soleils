using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpSunTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AmpTriggerInFusee.instance.Kill();
        }
    }
}
