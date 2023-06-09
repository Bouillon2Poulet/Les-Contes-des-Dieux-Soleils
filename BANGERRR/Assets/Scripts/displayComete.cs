using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayComete : MonoBehaviour
{
    private Renderer objectRenderer; // Référence au composant Renderer de l'objet
    private bool hasTriggered = false; // Indicateur pour vérifier si le collider a déjà été déclenché

    void Start()
    {
        objectRenderer = GetComponent<Renderer>(); // Obtient le composant Renderer attaché à cet objet
        objectRenderer.enabled=false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered)
        {
            hasTriggered = true; // Marque le collider comme déclenché
            objectRenderer.enabled = !objectRenderer.enabled; 
            Debug.Log("Collision avec : " + other.gameObject.name); // Affiche le nom de l'objet en collision dans la console d'Unity
        }
    }

    void OnTriggerExit(Collider other)
    {
        hasTriggered = false; // Réinitialise l'indicateur lorsque le SphereCollider n'est plus en collision
    }
}
