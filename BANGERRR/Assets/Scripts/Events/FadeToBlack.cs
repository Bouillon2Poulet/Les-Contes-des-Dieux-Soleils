using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public GameObject blackSquare;
    public GameObject whiteSquare;
    public bool fading = false;

    public void FadeInBlack(float speed)
    {
        StartCoroutine(Fade(true, speed));
    }
    public void FadeOutBlack(float fadespeed)
    {
        StartCoroutine(Fade(false, fadespeed));
    }

    public void FadeInWhite(float speed)
    {
        StartCoroutine(FadeWhiteEdition(true, speed));
    }
    public void FadeOutWhite(float fadespeed)
    {
        StartCoroutine(FadeWhiteEdition(false, fadespeed));
    }

    public IEnumerator Fade(bool fadeToBlack = true, float fadeSpeed = 2)
    {
        fading = true;

        Color objectColor = blackSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            Debug.Log("FadeIn!!");
            while (blackSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        } else
        {
            Debug.Log("FadeOut!!");
            while (blackSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }

        fading = false;
        yield break;
    }

    public IEnumerator FadeWhiteEdition(bool fadeToBlack = true, float fadeSpeed = 2)
    {
        fading = true;
        float fadeAmount;

        if (fadeToBlack)
        {
            fadeAmount = 0f;
            while (fadeAmount < 1)
            {
                fadeAmount += fadeSpeed * Time.deltaTime;
                whiteSquare.GetComponent<CanvasGroup>().alpha = fadeAmount;
                yield return null;
            }
            whiteSquare.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            fadeAmount = 1f;
            while (fadeAmount > 0)
            {
                fadeAmount -= fadeSpeed * Time.deltaTime;
                whiteSquare.GetComponent<CanvasGroup>().alpha = fadeAmount;
                yield return null;
            }
            whiteSquare.GetComponent<CanvasGroup>().alpha = 0f;
        }

        fading = false;
        yield break;
    }

    private void Start()
    {
        whiteSquare.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public static FadeToBlack instance { get; private set; }
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
