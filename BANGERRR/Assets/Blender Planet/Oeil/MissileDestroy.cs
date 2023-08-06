using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDestroy : MonoBehaviour
{
    public bool invincible = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!invincible)
        {
            Debug.Log("Collision !");
            GameObject otherGameObject = other.gameObject;
            if (otherGameObject.name == "Centre")
            {
                //Debug.Log("Centre touché");
                Centre.instance.Hit();
                Destroy(gameObject); // Détruire le GameObject spécifique
            }
            else if (otherGameObject.name == "Paupière_up" || otherGameObject.name == "Paupière_down")
            {
                //Debug.Log("Paupière touchée");
                Centre.instance.Hit();
                Destroy(gameObject); // Détruire le GameObject spécifique
            }
            else if (otherGameObject.name == "Third Person Player")
            {
                Debug.Log("Target touchée");
                Destroy(gameObject); // Détruire le GameObject spécifique
            }
        }
    }
}

