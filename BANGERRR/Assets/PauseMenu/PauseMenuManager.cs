using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject OptionsUI;
    public static Canvas PauseCanvas;

    public AudioMixer audioMixer;

    void Start()
    {
        PauseCanvas = GetComponent<Canvas>();
        PauseCanvas.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !PlayerStatus.instance.isAnimated)
        {
            Switch();
            AudioManager.instance.Play("openpause");
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

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetFxVolume(float volume)
    {
        audioMixer.SetFloat("fxVolume", Mathf.Log10(volume) * 20);
    }
}
