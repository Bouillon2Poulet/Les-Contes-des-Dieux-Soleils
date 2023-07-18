using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public GameObject blackSquare;

    public void FadeInBlack(float speed)
    {
        StartCoroutine(Fade(true, speed));
    }
    public void FadeOutBlack(float fadespeed)
    {
        StartCoroutine(Fade(false, fadespeed));
    }

    public IEnumerator Fade(bool fadeToBlack = true, float fadeSpeed = 2)
    {
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
        yield break;
    }
}