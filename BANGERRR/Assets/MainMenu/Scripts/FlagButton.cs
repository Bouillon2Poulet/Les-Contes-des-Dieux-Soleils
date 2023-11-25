using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagButton : MonoBehaviour
{
    [SerializeField] private LanguageManager.Lang lang;

    public void ToggleLang()
    {
        LanguageManager.instance.ToggleLang(lang);
    }
}
