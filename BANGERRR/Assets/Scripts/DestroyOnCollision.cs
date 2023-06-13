using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private GameObject amphipolisObject; // Référence à l'objet "Amphipolis"
    private GameObject planetObject; // Référence à l'objet "Planet"

    private void Start()
    {
        amphipolisObject = GameObject.Find("Amphipolis"); // Trouve l'objet "Amphipolis" dans la scène
        if (amphipolisObject != null)
        {
            planetObject = amphipolisObject.transform.Find("Planet").gameObject; // Trouve l'objet "Planet" enfant de "Amphipolis"
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == planetObject)
        {
            Destroy(gameObject); // Détruit l'objet courant
        }
    }
}
