using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] Sounds;

    public AudioMixerGroup MusicGroup;
    public AudioMixerGroup FxGroup;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
            
            if (s.music)
                s.source.outputAudioMixerGroup = MusicGroup;
            else
                s.source.outputAudioMixerGroup = FxGroup;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        s.source.Stop();
    }

    public void FadeIn(string name, int frames)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        float initialVolume = s.source.volume;
        s.source.volume = 0;
        s.source.Play();

        StartCoroutine(FadeInVolume(s.source, initialVolume, frames));
    }

    IEnumerator FadeInVolume(AudioSource source, float initial, int frames)
    {
        Debug.Log("Start FadeIn");
        for (int i = 0; i < frames; i++)
        {
            source.volume = i * initial / frames;
            yield return new WaitForEndOfFrame();
        }
        source.volume = initial;
        Debug.Log("End FadeIn");
        yield return null;
    }

    public void FadeOut(string name, int frames)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        float initialVolume = s.source.volume;
        StartCoroutine(FadeOutVolume(s.source, initialVolume, frames));
    }

    IEnumerator FadeOutVolume(AudioSource source, float initial, int frames)
    {
        Debug.Log("Start FadeOut");
        for (int i = 0; i < frames; i++)
        {
            source.volume = (frames-i) * initial / frames;
            yield return new WaitForEndOfFrame();
        }
        source.volume = 0;
        source.Stop();
        source.volume = initial;
        Debug.Log("End FadeOut");
        yield return null;
    }

    public bool isItPlaying(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return false;
        }

        return s.source.isPlaying;
    }

    public void StopAllMusic()
    {
        Sound[] musics = Array.FindAll(Sounds, sound => sound.music);
        foreach(Sound m in musics)
        {
            if (isItPlaying(m.name))
            {
                FadeOut(m.name, 60);
            }
        }
    }

    public void StopAllNonMusicLoops()
    {
        Sound[] nonMusics = Array.FindAll(Sounds, sound => !sound.music);
        Sound[] loops = Array.FindAll(nonMusics, sound => sound.loop);
        foreach(Sound m in loops)
        {
            Stop(m.name);
        }
    }
}
