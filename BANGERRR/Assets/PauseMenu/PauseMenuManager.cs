using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject OptionsUI;
    public static Canvas PauseCanvas;

    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider fxSlider;

    void Start()
    {
        PauseCanvas = GetComponent<Canvas>();
        PauseCanvas.enabled = false;

        float PlayerMusicVolume = PlayerPrefs.GetFloat("musicVolume");
        musicSlider.value = PlayerMusicVolume;
        audioMixer.SetFloat("musicVolume", Mathf.Log10(PlayerMusicVolume) * 20);

        float PlayerFxVolume = PlayerPrefs.GetFloat("fxVolume");
        fxSlider.value = PlayerFxVolume;
        audioMixer.SetFloat("fxVolume", Mathf.Log10(PlayerFxVolume) * 20);
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
        PlayerPrefs.SetFloat("musicVolume", volume);
        //Debug.Log("musicVolume : " + (PlayerPrefs.GetFloat("musicVolume")));
    }

    public void SetFxVolume(float volume)
    {
        audioMixer.SetFloat("fxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("fxVolume", volume);
        //Debug.Log("fxVolume : " + (PlayerPrefs.GetFloat("fxVolume")));
    }
}
