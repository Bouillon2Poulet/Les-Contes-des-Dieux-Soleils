using UnityEngine;

public class OrbitRenderer : MonoBehaviour
{
    public int resolution = 100; // Résolution du contour de l'orbite
    public Material lineMaterial; // Matériau à appliquer au LineRenderer

    private float semiMajorAxis;
    private float semiMinorAxis;
    private LineRenderer lineRenderer; // Référence au LineRenderer

    void Start()
    {
        // Ajouter le LineRenderer au même objet
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Définir les paramètres du LineRenderer
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.useWorldSpace = true; // Utiliser les coordonnées du monde pour les positions
        lineRenderer.startWidth = 5f;
        lineRenderer.endWidth = 5f;

        // Appliquer le matériau au LineRenderer
        lineRenderer.material = lineMaterial;

        semiMajorAxis = GetComponent<SimpleEllipseRotation>().semiMajorAxis;
        semiMinorAxis = GetComponent<SimpleEllipseRotation>().semiMinorAxis;

        // Tracer le contour de l'orbite
        UpdateOrbit();
    }

    void Update()
    {
        // Tracer le contour de l'orbite à chaque frame (si vous voulez que l'orbite se mette à jour en temps réel)
        UpdateOrbit();
    }

    void UpdateOrbit()
    {
        // Calculer l'angle entre chaque point du contour de l'orbite
        float angleIncrement = 2 * Mathf.PI / resolution;

        // Tableau de positions pour stocker les points du contour de l'orbite
        Vector3[] positions = new Vector3[resolution + 1];

        for (int i = 0; i <= resolution; i++)
        {
            // Calculer l'angle actuel
            float angle = i * angleIncrement;

            // Calculer la position du point sur l'orbite en utilisant les axes semi-majeur et semi-mineur
            float x = Mathf.Cos(angle) * semiMajorAxis;
            float z = Mathf.Sin(angle) * semiMinorAxis;

            // Définir la position du point par rapport au centre de l'orbite (position du composant parent)
            positions[i] = transform.parent.position + new Vector3(x, 0f, z);
        }

        // Assigner les positions au LineRenderer
        lineRenderer.SetPositions(positions);
    }
}
