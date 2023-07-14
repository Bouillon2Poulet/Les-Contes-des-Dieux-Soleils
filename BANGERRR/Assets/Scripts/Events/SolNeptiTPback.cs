using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolNeptiTPback : MonoBehaviour
{
    public Rigidbody Nepti;

    public void Teleport()
    {
        Nepti.position = transform.position;
        Debug.Log("Nepti is back");
    }
}
