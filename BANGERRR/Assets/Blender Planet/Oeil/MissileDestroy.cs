using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDestroy : MonoBehaviour
{
    // public GameObject target;
    // public Prefab centreObject;
    // public Prefab paupiereObject1;
    // public Prefab paupiereObject2;

    bool canBeDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if (canBeDestroyed)
        {
            GameObject otherGameObject = other.gameObject;
            if (otherGameObject.name == "Centre")
            {
                Debug.Log("Centre touché");
                Destroy(gameObject); // Détruire le GameObject spécifique
            }
            else if (otherGameObject.name == "Paupière_up" || otherGameObject.name == "Paupière_down")
            {
                Debug.Log("Paupière touchée");
                Destroy(gameObject); // Détruire le GameObject spécifique
            }
            else if (otherGameObject.name == "Target")
            {
                Debug.Log("Target touchée");
                Destroy(gameObject); // Détruire le GameObject spécifique
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canBeDestroyed = true;
    }
}

