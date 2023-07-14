using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolCoralTPback : MonoBehaviour
{
    public Rigidbody Coral;

    public void Teleport()
    {
        Coral.position = transform.position;
        Debug.Log("Coral is back");
    }
}
