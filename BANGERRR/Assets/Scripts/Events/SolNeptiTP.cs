using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolNeptiTP : MonoBehaviour
{
    public Rigidbody Nepti;

    public void Teleport()
    {
        Nepti.position = transform.position;
        Debug.Log("Nepti go cachette");
    }
}
