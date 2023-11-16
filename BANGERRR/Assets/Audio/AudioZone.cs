using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour
{
    public string musicName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!AudioManager.instance.isItPlaying(musicName))
            {
                AudioManager.instance.FadeIn(musicName, 120);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (AudioManager.instance.isItPlaying(musicName))
            {
                AudioManager.instance.FadeOut(musicName, 120);
            }
        }
    }
}
