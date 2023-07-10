using UnityEngine;

public class TrackBallCamera : MonoBehaviour
{
    public float rotationSpeed = 10f; // Vitesse de rotation de la caméra
    public float radius = 10f; // Rayon de déplacement de la caméra
    public OpenCosmoGuide OpenCosmoGuideScript;
    private float angle = 0f; // Angle de rotation de la caméra

    void Update()
    {
        if (OpenCosmoGuideScript.CosmoGuideIsOpen)
        {
            // Rotation de la caméra
            if (Input.GetKey(KeyCode.A) && OpenCosmoGuideScript.CosmoGuideIsOpen)
            {
                angle -= rotationSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                angle += rotationSpeed * Time.deltaTime;
            }

            // Calcul de la position de la caméra sur l'axe x en fonction de l'angle et du rayon
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = 500f; // Déplacement sur l'axe y de +500
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            // Déplacement de la caméra vers la nouvelle position
            transform.position = new Vector3(x, y, z);

            // Regarder vers le centre de la scène
            transform.LookAt(Vector3.zero);
        }
    }
}
