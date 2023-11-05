using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public GameObject mainCamera;

    public PlanetTag fin;

    private void Awake()
    {
        if (ChapterManager.maxChapterIndexDiscoveredByPlayer == 6)
        {
            fin.DiscoverPlanet();
        }
        ChapterManager.currentChapterIndex = 6;
    }

    void Start()
    {
        StartCoroutine(nameof(MakeMainCameraWork));
        //StartCoroutine(TheEnd());
    }

    IEnumerator TheEnd()
    {
        FadeToBlack.instance.whiteSquare.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(2f);
        yield return FadeToBlack.instance.FadeWhiteEdition(false, .1f);
        yield return new WaitForSeconds(3f);
        yield return FindObjectOfType<DialogManager>().EphemeralMessage(" ", "The end", 20f);
    }

    private IEnumerator MakeMainCameraWork()
    {
        yield return new WaitForSeconds(.2f);
        mainCamera.SetActive(false);
        yield return new WaitForSeconds(.2f);
        mainCamera.SetActive(true);
    }
}
