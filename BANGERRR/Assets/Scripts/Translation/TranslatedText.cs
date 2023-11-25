using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslatedText : MonoBehaviour
{
    [SerializeField] string french;
    [SerializeField] string english;

    private void Start()
    {
        if (PlayerPrefs.HasKey("lang"))
        {
            LanguageManager.Lang lang = (LanguageManager.Lang)GlobalVariables.Get<int>("lang");
            InitText(lang);
        }
    }

    public void InitText(LanguageManager.Lang lang)
    {
        if (lang == LanguageManager.Lang.French)
        {
            GetComponent<TextMeshProUGUI>().text = french;
        }
        else if (lang == LanguageManager.Lang.English)
        {
            GetComponent<TextMeshProUGUI>().text = english;
        }
    }
}
