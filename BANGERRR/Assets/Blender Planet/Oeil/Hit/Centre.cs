using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centre : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Color origColor;
    readonly float flashTime = .25f;
    Material mat;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        mat = meshRenderer.materials[1];
        mat.EnableKeyword("_EmissionColor");

        origColor = Color.white;
    }

    public void Hit()
    {
        Debug.Log("Hit Flash!");
        StartCoroutine(EFlash());
    }

    IEnumerator EFlash()
    {
        mat.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(flashTime);
        mat.SetColor("_EmissionColor", origColor);
    }


    public static Centre instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
