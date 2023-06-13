using UnityEngine;

public class TrailRendererController : MonoBehaviour
{
    public MeshRenderer meshRenderer; // Référence au MeshRenderer de l'objet
    public float widthMultiplier = 0.5f; // Multiplicateur de largeur du TrailRenderer

    private TrailRenderer trailRenderer; // Référence au TrailRenderer de l'objet

    void Start()
    {
        // Récupérer la référence au TrailRenderer
        trailRenderer = GetComponent<TrailRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Assigner le même matériau que le MeshRenderer
        trailRenderer.material = meshRenderer.material;
    }

    void Update()
    {
        // Mettre à jour la largeur du TrailRenderer en fonction de l'échelle de l'objet
        trailRenderer.widthMultiplier = transform.localScale.magnitude * widthMultiplier;
    }
}
