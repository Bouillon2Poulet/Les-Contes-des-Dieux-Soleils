using UnityEngine;

public class Aspire : MonoBehaviour
{
    public float speed = 1.0f;
    public float maxScaleFactor = 1.2f;
    public ParticleSystem particleSystemPrefab; // Le prefab du ParticleSystem à instancier

    private Vector3 initialScale;
    private float timeOffset;
    private bool animationStarted = false;
    private float timeStart;
    private ParticleSystem currentParticleSystem; // La référence vers l'instance du ParticleSystem actuel

    void Start()
    {
        initialScale = new Vector3(1f, 1f, 1f);
        timeOffset = Random.Range(0f, 2f * Mathf.PI); // Random offset for variety
    }

    void Update()
    {
        // Check for left mouse button click to start the animation
        if (Input.GetKeyDown(KeyCode.Alpha2) && !animationStarted)
        {
            animationStarted = true;
            timeStart = Time.time;

            // Instancier le ParticleSystem et le stocker dans la variable currentParticleSystem
            currentParticleSystem = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
        }

        if (animationStarted)
        {
            // Calculate the scale factor using a sin function to create variation in the scale change
            float scaleFactor = Mathf.Sin((Time.time - timeStart) * speed) * 0.5f + 0.5f; // Maps sin output from [-1, 1] to [0, 1]
            scaleFactor = Mathf.Clamp(scaleFactor, 0f, 1f); // Clamp to [0, 1]

            // Apply the scale factor to the initial scale to get the new scale
            Vector3 newScale = initialScale * (1f + scaleFactor * (maxScaleFactor - 1f));

            // Apply the new scale to the object
            transform.localScale = newScale;
            
            // Check if the currentParticleSystem is finished playing and destroy the object if it is
            if (currentParticleSystem != null && !currentParticleSystem.isPlaying)
            {
                Destroy(currentParticleSystem.gameObject);
            }

            if (scaleFactor < 0.1 && Mathf.Cos((Time.time - timeStart) * speed) < 0f)
            {
                animationStarted = false;
                transform.localScale = initialScale;
            }
        }
    }
}
