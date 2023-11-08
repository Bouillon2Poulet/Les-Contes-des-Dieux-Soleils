using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject OptionsUI;
    static Canvas PauseCanvas;

    void Start()
    {
        PauseCanvas = GetComponent<Canvas>();
        PauseCanvas.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !PlayerStatus.instance.isAnimated && !DialogManager.instance.isItActive())
        {
            Switch();

            // Hide the options just in case
            OptionsUI.SetActive(false);
        }
    }

    public static void Switch()
    {
        bool newState = !PauseCanvas.enabled;

        // Activate or deactivate canvas
        PauseCanvas.enabled = newState;

        // Cursor management
        PlayerStatus.instance.GameMenuCursor(newState);

        // Change the interaction icon
        KeyInteractionManager.instance.gameObject.SetActive(!newState);
    }
}
