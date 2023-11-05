using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemTimeToShader : MonoBehaviour
{
    ParticleSystem myParticleSystem;
    Material myMaterial;

    private void Awake()
    {
        myParticleSystem = gameObject.GetComponent<ParticleSystem>();
        myMaterial = gameObject.GetComponent<ParticleSystemRenderer>().material;
    }

    void Update()
    {
        int particletime = (int)Mathf.Floor(myParticleSystem.time);
        myMaterial.SetFloat("_particlesystemtime", particletime);   
    }
}
