using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpSunTrigger : MonoBehaviour
{
    public GameObject Loeil;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AmpTriggerInFusee.instance.Kill();
            Loeil.SetActive(true);
            Destroy(gameObject);
        }
    }
}
