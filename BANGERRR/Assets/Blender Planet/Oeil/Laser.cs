using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineInstantiator : MonoBehaviour
{
    public GameObject linePrefab; // Le prefab de la Line à instancier
    public float speed = 1;
    GameObject lineInstance;

    public GameObject Iris;

    public bool laserGoing = false;
    public bool hit = false;

    private void StartLaser()
    {
        // Vérifier si le prefab de la Line est défini
        if (linePrefab == null)
        {
            Debug.LogError("Veuillez définir le prefab de la Line dans l'inspecteur du script.");
            return;
        }

        // Instancier la Line
        lineInstance = Instantiate(linePrefab, transform);

        lineInstance.transform.localPosition = new Vector3(0f, 0f, 0f);

        // Obtenir une référence du Line Renderer
        LineRenderer lineRenderer = lineInstance.GetComponent<LineRenderer>();

        // Obtenir une référence du BoxCollider
        BoxCollider boxCollider = lineInstance.GetComponent<BoxCollider>();

        // Incrémenter la position et faire osciller la Width
        //StartCoroutine(AnimatePosition(lineRenderer, boxCollider));
        //StartCoroutine(AnimateWidth(lineRenderer));
    }

    private void StopLaser()
    {
        StopAllCoroutines();
        laserGoing = false;
        Destroy(lineInstance);
    }

    bool isIrisAnimationFinished = false;
    private void IrisAnimate()
    {
        isIrisAnimationFinished = false;
        StartCoroutine(IrisAnimation());
    }

    private IEnumerator IrisAnimation()
    {
        Vector3 initialScale = new Vector3(.666f, .666f, .1f);
        Vector3 targetScale = new Vector3(0f, 0f, .1f);
        
        float animationDuraction = 3f;
        float elapsedTime = 0f;

        Iris.transform.localScale = initialScale;
        Iris.SetActive(true);

        while (elapsedTime < animationDuraction)
        {
            float t = elapsedTime / animationDuraction;
            Iris.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Iris.SetActive(false);
        isIrisAnimationFinished = true;
    }

    private IEnumerator StartLaserForAmountOfSeconds(float duration)
    {
        hit = false;
        IrisAnimate();
        yield return new WaitUntil(() => isIrisAnimationFinished);
        StartLaser();
        AudioManager.instance.Play("omnio_laserloop");

        float endTime = Time.time + duration;
        while (Time.time < endTime && !hit)
        {
            yield return null;
        }

        StopLaser();
        AudioManager.instance.Stop("omnio_laserloop");
        if (hit)
        {
            Debug.Log("Brûlé !!");
            PlayerStatus.instance.HitBlink();
            PhaseManager.instance.TriggerReset();
        }
    }

    public void Trigger(float duration)
    {
        laserGoing = true;
        StartCoroutine(StartLaserForAmountOfSeconds(duration));
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

    public static LineInstantiator instance { get; private set; }
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
