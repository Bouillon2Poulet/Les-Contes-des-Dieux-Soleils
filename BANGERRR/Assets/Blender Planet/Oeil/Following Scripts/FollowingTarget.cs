using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingTarget : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Utilise LookAt pour orienter la rotation de l'objet vers la cible
            transform.LookAt(target.transform);
        }
    }
}