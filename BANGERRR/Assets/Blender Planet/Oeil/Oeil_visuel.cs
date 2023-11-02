using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oeil_visuel : MonoBehaviour
{
    public Renderer PaupiereUp;
    public Renderer PaupiereDown;
    public Renderer CentreRenderer;
    public float hueRotationSpeed = 60.0f;

    private Material paupiereUpMat;
    private Material paupiereDownMat;
    private Material CentreMat;
    private float hueValue = 0.0f;

    private void Start()
    {
        CentreMat = CentreRenderer.materials[2];
        paupiereUpMat = PaupiereUp.material;
        paupiereDownMat = PaupiereDown.material;
        CentreMat.EnableKeyword("_EmissionColor");
        paupiereUpMat.EnableKeyword("_EmissionColor");
        paupiereDownMat.EnableKeyword("_EmissionColor");
    }

    void Update()
    {
        hueValue = (hueValue + hueRotationSpeed * Time.deltaTime) % 360.0f;

        Color newColor = Color.HSVToRGB(hueValue / 360.0f, 1.0f, 1.0f);
        CentreMat.SetColor("_EmissionColor", newColor);
        paupiereUpMat.SetColor("_EmissionColor", newColor);
        paupiereDownMat.SetColor("_EmissionColor", newColor);
    }
}
