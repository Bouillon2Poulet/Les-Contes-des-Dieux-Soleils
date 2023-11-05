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
            bool newState = !GetComponent<Canvas>().enabled;

            // Activate or deactivate canvas
            GetComponent<Canvas>().enabled = newState;

            // Cursor management
            PlayerStatus.instance.GameMenuCursor(newState);

            // Change the interaction icon
            KeyInteractionManager.instance.ChangeMenuIcon(newState);

            // Hide the options just in case
            OptionsUI.SetActive(false);
        }
    }
}
