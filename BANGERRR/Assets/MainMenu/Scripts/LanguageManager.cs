using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public enum Lang
    {
        French,
        English
    }

    public GameObject FlagFr;
    public GameObject FlagEn;

    private void Start()
    {
        InitLang();
        ToggleLang((Lang)GlobalVariables.Get<int>("lang"));
    }

    public void InitLang()
    {
        if (PlayerPrefs.HasKey("lang"))
        {
            int lang = PlayerPrefs.GetInt("lang");
            GlobalVariables.Set("lang", lang);
        }
        else
        {
            PlayerPrefs.SetInt("lang", (int)Lang.French);
            GlobalVariables.Set("lang", (int)Lang.French);
        }
    }

    public void ToggleLang(Lang lang)
    {
        PlayerPrefs.SetInt("lang", (int)lang);
        GlobalVariables.Set("lang", (int)lang);

        FlagFr.transform.GetChild(0).gameObject.SetActive(lang == Lang.French);
        FlagEn.transform.GetChild(0).gameObject.SetActive(lang == Lang.English);

        foreach(TranslatedSprite s in FindObjectsByType<TranslatedSprite>(FindObjectsSortMode.None))
        {
            s.ToggleSprite(lang);
        }

        ChapterSelectionManager.instance.UpdateTitlesLang(lang);

        Debug.Log("Setting Lang to " + lang);
    }

    public static LanguageManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
