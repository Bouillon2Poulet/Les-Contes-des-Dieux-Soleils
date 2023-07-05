using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEllipseRotation : MonoBehaviour
{   
    public float semiMajorAxis;
    public float semiMinorAxis;
    public float speed = 1;
    public float offset;
    public bool invert;

    // Start is called before the first frame update
    void Start()
    {
        int invertFactor = (invert) ? -1 : 1;
        transform.position = GetPosition(GetComponentInParent<SystemDayCounter>().systemTime, invertFactor); // Met à jour la position de l'objet sur l'ellipse
    }
    // Update is called once per frame
    void Update()
    {
        int invertFactor = (invert) ? -1 : 1;
        transform.position = GetPosition(GetComponentInParent<SystemDayCounter>().systemTime, invertFactor); // Met à jour la position de l'objet sur l'ellipse
    }

    Vector3 GetPosition(float time, int invertFactor){
        float angleBySecond = (speed * 2f * Mathf.PI)/86400f; //Nombre secondes dans une journée
        float currentAngle = offset + invertFactor * time * angleBySecond;
        float x = transform.parent.position.x + semiMajorAxis * Mathf.Cos(currentAngle); // Calcul de la coordonnée x sur l'ellipse avec décalage de l'objet parent
        float z = transform.parent.position.z + semiMinorAxis * Mathf.Sin(currentAngle); // Calcul de la coordonnée z sur l'ellipse avec décalage de l'objet parent
        return new Vector3(x, 0f, z);
    }
}
