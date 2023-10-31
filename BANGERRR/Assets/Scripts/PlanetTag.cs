using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// aka PlanetProgressController
/// </summary>
public class PlanetTag : MonoBehaviour
{
    static GameObject ui;
    static Image image;
    static CanvasGroup canvasGroup;

    static readonly float tagOnScreenSeconds = 6f;

    bool hasBeenTriggered = false;

    [SerializeField] GameObject planet;
    [SerializeField] Sprite sprite;
    [SerializeField] GameObject[] toActivate;
    [SerializeField] GameObject[] toDeactivate;

    private bool isSolisede;

    private void Start()
    {
        if (image == null)
        {
            ui = GameObject.FindGameObjectWithTag("PlanetTag");
            image = ui.GetComponent<Image>();
            canvasGroup = ui.GetComponent<CanvasGroup>();
        }

        ui.SetActive(false);
        canvasGroup.alpha = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenTriggered)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //Debug.Log("PT");
                hasBeenTriggered = true;
                if (isSolisede)
                    PlayerStatus.instance.LooseBubble();
                StartCoroutine(ShowTag());
            }
        }
    }

    public void DiscoverPlanet()
    {
        //Debug.Log("Discovering " + planet.name);
        planet.GetComponent<HasBeenDiscovered>().state = true;
    }

    public void DeactivateObjects()
    {
        foreach (GameObject o in toDeactivate)
        {
            o.SetActive(false);
        };
    }

    public void ActivateObjects()
    {
        foreach (GameObject o in toActivate)
        {
            o.SetActive(true);
        };
    }

    private IEnumerator ShowTag()
    {
        image.sprite = sprite;
        canvasGroup.alpha = 0f;
        ui.SetActive(true);

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

        ui.SetActive(false);
    }
}
