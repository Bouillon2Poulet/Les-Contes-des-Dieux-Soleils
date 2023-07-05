using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEllipseRotationTristan : MonoBehaviour
{   
    public float semiMajorAxis;
    public float semiMinorAxis;
    public float offset;
    public bool invert;
    public int speed = 1;
    private float vitesseRadiale;
    private float currentAngle;

    // Start is called before the first frame update
    void Start()
    {
        int invertFactor = (invert) ? -1 : 1;
        currentAngle = offset;
        vitesseRadiale = (speed * 2f * Mathf.PI)/GetComponentInParent<SystemDayCounter>().oneDayDurationInIRLSeconds;
        transform.position = GetPosition(invertFactor); // Met à jour la position de l'objet sur l'ellipse
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        int invertFactor = (invert) ? -1 : 1;
        currentAngle += invertFactor * vitesseRadiale * Time.deltaTime;
        transform.position = GetPosition(invertFactor); // Met à jour la position de l'objet sur l'ellipse
    }

    Vector3 GetPosition(int invertFactor){
        float x = transform.parent.position.x + semiMajorAxis * Mathf.Cos(currentAngle); // Calcul de la coordonnée x sur l'ellipse avec décalage de l'objet parent
        float z = transform.parent.position.z + semiMinorAxis * Mathf.Sin(currentAngle); // Calcul de la coordonnée z sur l'ellipse avec décalage de l'objet parent
        return new Vector3(x, 0f, z);
    }
}
