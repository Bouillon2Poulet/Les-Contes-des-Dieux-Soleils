using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpEyeActivator : MonoBehaviour
{
    public GameObject Loeil;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Loeil.SetActive(true);
        }
    }
}
