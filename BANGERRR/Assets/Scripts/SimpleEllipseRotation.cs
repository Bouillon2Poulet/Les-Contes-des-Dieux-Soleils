using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEllipseRotation : MonoBehaviour
{
    public GameObject orbitObject; // Référence à l'objet "Orbite"
    public float speed = 1;
    public float offset;

    public bool invert;

    // Start is called before the first frame update
    void Start()
    {
        float x = orbitObject.transform.localScale.x/2f; // Récupère l'échelle (scale) x de l'objet "Orbite"
        float z = orbitObject.transform.localScale.z/2f; // Récupère l'échelle (scale) z de l'objet "Orbite"
        transform.position = (new Vector3(x, 0f,z)); // Met à jour la position de l'objet sur l'ellipse
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time; // Temps écoulé depuis le démarrage du jeu

        int invertFactor = (invert) ? -1 : 1;
        float angle = offset + invertFactor * time * speed / 100f * Mathf.PI * 2f; // Conversion du temps en angle

        float x, z;

        float parentX = orbitObject.transform.position.x; // Coordonnée x de l'objet parent
        float parentZ = orbitObject.transform.position.z; // Coordonnée z de l'objet parent

        float horizontalAxis = orbitObject.transform.localScale.x/2f; // Récupère l'échelle (scale) x de l'objet "Orbite"
        float verticalAxis = orbitObject.transform.localScale.z/2f; // Récupère l'échelle (scale) z de l'objet "Orbite"

        x = parentX + horizontalAxis * Mathf.Cos(angle); // Calcul de la coordonnée x sur l'ellipse avec décalage de l'objet parent
        z = parentZ + verticalAxis * Mathf.Sin(angle); // Calcul de la coordonnée z sur l'ellipse avec décalage de l'objet parent

        transform.position = new Vector3(x, 0f, z); // Met à jour la position de l'objet sur l'ellipse
    }
}
