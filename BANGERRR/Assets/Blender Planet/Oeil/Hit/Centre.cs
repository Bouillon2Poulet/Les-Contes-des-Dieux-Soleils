using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centre : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Color origColor;
    readonly float flashTime = .25f;
    Material mat;
    Aspire aspire;
    public bool hit = false;

    public bool playerTouched;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        mat = meshRenderer.materials[1];
        mat.EnableKeyword("_EmissionColor");

        origColor = Color.white;

        aspire = FindObjectOfType<Aspire>();
    }

    public void Hit()
    {
        hit = true;
        //Debug.Log("Hit Flash!");
        StartCoroutine(EFlash());

        // Destroy all other missiles
        MissileDestroy[] missiles = FindObjectsByType<MissileDestroy>(FindObjectsSortMode.None);
        foreach (MissileDestroy missile in missiles)
        {
            Destroy(missile.gameObject);
        }

        // Aspire
        aspire.Trigger();
    }

    IEnumerator EFlash()
    {
        mat.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(flashTime);
        mat.SetColor("_EmissionColor", origColor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTouched = true;
        }
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
