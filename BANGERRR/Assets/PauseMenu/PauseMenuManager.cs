using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject OptionsUI;

    void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !PlayerStatus.instance.isAnimated && !DialogManager.instance.isItActive())
        {
            GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
            PlayerStatus.instance.GameMenuCursor(GetComponent<Canvas>().enabled);
            OptionsUI.SetActive(!GetComponent<Canvas>().enabled);
        }
    }
}
