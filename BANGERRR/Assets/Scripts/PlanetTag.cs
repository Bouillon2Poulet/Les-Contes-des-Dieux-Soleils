using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetTag : MonoBehaviour
{
    static Image image;
    static CanvasGroup canvasGroup;

    static readonly float tagOnScreenSeconds = 6f;

    bool hasBeenTriggered = false;

    [SerializeField] GameObject planet;
    [SerializeField] Sprite sprite;
    [SerializeField] GameObject[] toActivate;
    [SerializeField] GameObject[] toDeactivate;

    private void Start()
    {
        if (image == null)
        {
            GameObject ui = GameObject.FindGameObjectWithTag("PlanetTag");
            image = ui.GetComponent<Image>();
            canvasGroup = ui.GetComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenTriggered)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                hasBeenTriggered = true;
                planet.GetComponent<HasBeenDiscovered>().state = true;
                // activate objects 
                // desactivate objects 
                StartCoroutine(ShowTag());
            }
        }
    }

    private IEnumerator ShowTag()
    {
        image.sprite = sprite;
        canvasGroup.alpha = 0f;

        float fadingSpeed = .025f;
        float fadingProgression = 0f;

        while (fadingProgression < 1f)
        {
            canvasGroup.alpha = fadingProgression;
            fadingProgression += fadingSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(tagOnScreenSeconds);

        while (fadingProgression > 0f)
        {
            canvasGroup.alpha = fadingProgression;
            fadingProgression -= fadingSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
