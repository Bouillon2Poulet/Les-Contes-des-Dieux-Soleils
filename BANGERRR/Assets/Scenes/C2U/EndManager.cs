using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
}
