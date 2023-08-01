using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpDoorOpeningTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AmpAscenseur.instance.OpenTheDoor();
            gameObject.SetActive(false);
        }
    }
}
