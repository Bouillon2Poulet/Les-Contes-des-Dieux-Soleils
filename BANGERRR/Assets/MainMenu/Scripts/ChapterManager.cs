using System;
using System.IO;
using UnityEngine;

static class ChapterManager
{
    public static int currentChapterIndex = 0;
    public static int maxChapterIndexDiscoveredByPlayer = 0;

    public enum Chapter
    {
        triton,
        eaux_divine,
        solisède,
        solimont,
        amphipolis,
        oeil,
        fin,
        size
    }

    [Serializable] // Add this attribute to make the class serializable
    public class SaveObject
    {
        public Chapter currentChapter;
        public Chapter maxChapterDiscovered;
    }

    public static void InitPlayerPrefs()
    {
        PlayerPrefs.SetInt("currentChapter", (int)Chapter.triton);
        PlayerPrefs.SetInt("maxChapterDiscovered", (int)Chapter.triton);
    }

    public static void NewChapterDiscovered()
    {
        maxChapterIndexDiscoveredByPlayer++;
        Debug.Log("New chapter discovered: " + maxChapterIndexDiscoveredByPlayer);
        SaveProgression();
    }

    public static void SaveProgression()
    {
        PlayerPrefs.SetInt("currentChapter", currentChapterIndex);
        PlayerPrefs.SetInt("maxChapterDiscovered", maxChapterIndexDiscoveredByPlayer);
    }

    public static void GetSave()
    {
        currentChapterIndex = PlayerPrefs.GetInt("currentChapter");
        maxChapterIndexDiscoveredByPlayer = PlayerPrefs.GetInt("maxChapterDiscovered");
    }

    public static void ResetProgression()
    {
        PlayerPrefs.SetInt("0", (int)Chapter.triton);
        PlayerPrefs.SetInt("0", (int)Chapter.triton);

        currentChapterIndex = 0;
        maxChapterIndexDiscoveredByPlayer = 0;
    }
}
