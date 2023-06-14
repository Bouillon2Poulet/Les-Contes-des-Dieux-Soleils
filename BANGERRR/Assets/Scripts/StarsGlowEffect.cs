using UnityEngine;

public class StarsGlowEffect : MonoBehaviour
{
    public float sizeSpeed = 1f; // Vitesse de la variation de taille
    public float minSize = 1f; // Taille minimale des particules
    public float maxSize = 4f; // Taille maximale des particules

    private ParticleSystem particleSystem; // Référence au système de particules

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>(); // Récupère le composant ParticleSystem attaché à cet objet
    }

    private void Update()
    {
        // Calcule la nouvelle taille des particules en fonction du temps
        float newSize = Mathf.Lerp(minSize, maxSize, (Mathf.Sin(Time.time * sizeSpeed) + 1f) / 2f);

        // Récupère les données des particules du système de particules
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
        int particleCount = particleSystem.GetParticles(particles);

        // Met à jour la taille de chaque particule
        for (int i = 0; i < particleCount; i++)
        {
            particles[i].startSize = newSize;
        }

        // Applique les modifications aux particules du système de particules
        particleSystem.SetParticles(particles, particleCount);
    }
}
