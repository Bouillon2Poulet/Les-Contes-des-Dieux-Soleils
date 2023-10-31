using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliGrassVisibility : MonoBehaviour
{
    public GameObject planes;

    private void Awake()
    {
        planes.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            planes.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            planes.SetActive(false);
        }
    }
}
