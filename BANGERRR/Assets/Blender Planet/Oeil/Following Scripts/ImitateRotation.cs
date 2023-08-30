using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImitateRotation : MonoBehaviour
{
    public GameObject target;
    public float speed = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Calcule la rotation que l'objet doit atteindre pour imiter la rotation de la cible
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.forward, target.transform.up);

            // Effectue une interpolation spherique pour adoucir le mouvement de rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }
}