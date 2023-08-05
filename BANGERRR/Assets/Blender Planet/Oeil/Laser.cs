using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineInstantiator : MonoBehaviour
{
    public GameObject linePrefab; // Le prefab de la Line à instancier
    public float speed = 1;

    private void Start()
    {
        // Vérifier si le prefab de la Line est défini
        if (linePrefab == null)
        {
            Debug.LogError("Veuillez définir le prefab de la Line dans l'inspecteur du script.");
            return;
        }

        // Instancier la Line et la placer en tant qu'enfant du GameObject courant
        GameObject lineInstance = Instantiate(linePrefab, transform);

        // Définir la position de l'instantiation à (0, 1, 0)
        lineInstance.transform.position = new Vector3(0f, 0f, 0f);

        // Obtenir une référence du Line Renderer attaché à l'objet instantié
        LineRenderer lineRenderer = lineInstance.GetComponent<LineRenderer>();

        // Obtenir une référence du BoxCollider attaché à l'objet instantié
        BoxCollider boxCollider = lineInstance.GetComponent<BoxCollider>();

        // Lancer les Coroutines pour incrémenter la position et faire osciller la Width
        StartCoroutine(AnimatePosition(lineRenderer, boxCollider));
        StartCoroutine(AnimateWidth(lineRenderer));
    }

    private IEnumerator AnimatePosition(LineRenderer lineRenderer, BoxCollider boxCollider)
    {
        // Incrémenter la position Z de l'index 0 de manière continue jusqu'à 10
        while (lineRenderer.GetPosition(0).z < 10f)
        {
            Vector3 pos = lineRenderer.GetPosition(0);
            pos.z += speed * Time.deltaTime;
            lineRenderer.SetPosition(0, pos);

            // Ajuster la position du BoxCollider
            boxCollider.center = new Vector3(0f, 0f, pos.z / 2f);

            // Ajuster la taille du BoxCollider
            Vector3 size = boxCollider.size;
            size.z = pos.z;
            boxCollider.size = size;

            yield return null;
        }
    }

    private IEnumerator AnimateWidth(LineRenderer lineRenderer)
    {
        // Osciller la Width de manière infinie entre 0.13 et 0.16
        float min = 0.13f;
        float max = 0.16f;
        float duration = 1.5f;
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime / duration;
            float width = Mathf.Lerp(min, max, t);
            lineRenderer.widthMultiplier = width;

            if (t >= 1f)
            {
                t = 0f;
                float temp = min;
                min = max;
                max = temp;
            }

            yield return null;
        }
    }
}
