using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class omnio_eye : MonoBehaviour
{
    public Blink upBlink;
    public Blink downBlink;

    public Renderer PaupiereUp;
    public Renderer PaupiereDown;
    public Renderer CentreRenderer;
    public float hueRotationSpeed = 200.0f;

    private float hueValue = 0.0f;
    private Material paupiereUpMat;
    private Material paupiereDownMat;
    private Material CentreMat;

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

    private IEnumerator Blink(int amount, float mo)
    {
        BlinkEye(amount);
        yield return new WaitUntil(() => !upBlink.IsBlinking());
        yield return new WaitForSeconds(mo);
    }

    private void BlinkEye(int amount)
    {
        upBlink.Trigger(amount);
        downBlink.Trigger(amount);
    }
}
