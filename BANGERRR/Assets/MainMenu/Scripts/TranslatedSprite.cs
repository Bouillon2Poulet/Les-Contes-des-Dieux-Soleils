using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslatedSprite : MonoBehaviour
{
    public Sprite French;
    public Sprite English;

    private void Start()
    {
        if (PlayerPrefs.HasKey("lang"))
        {
            LanguageManager.Lang lang = (LanguageManager.Lang)GlobalVariables.Get<int>("lang");
            ToggleSprite(lang);
        }
    }

    public void ToggleSprite(LanguageManager.Lang lang)
    {
        if (lang == LanguageManager.Lang.French)
        {
            GetComponent<Image>().sprite = French;
        }
        else if (lang == LanguageManager.Lang.English)
        {
            GetComponent<Image>().sprite = English;
        }
    }
}
