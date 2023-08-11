using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliLight : MonoBehaviour
{
    public Color larmeColor;
    
    private Color solimontColor;

    void Start()
    {
        solimontColor = gameObject.GetComponent<Light>().color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Larme")
        {
            StartCoroutine(LerpColor(solimontColor, larmeColor));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Larme")
        {
            StartCoroutine(LerpColor(larmeColor, solimontColor));
        }
    }

    private IEnumerator LerpColor(Color startColor, Color endColor)
    {
        float fadingProgression = 0f;
        float fadingSpeed = .02f;

        while (fadingProgression < 1f)
        {
            Color lerpedColor = Color.Lerp(startColor, endColor, fadingProgression);
            fadingProgression += fadingSpeed;

            gameObject.GetComponent<Light>().color = lerpedColor;
            yield return null;
        }
    }
}
