using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpSunTrigger : MonoBehaviour
{
    public GameObject SUNLIGHT;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AmpTriggerInFusee.instance.Kill();
            SUNLIGHT.SetActive(false);
            ThirdPersonMovement.ToggleBulleJetpack(true);
            Destroy(gameObject);
        }
    }
}
