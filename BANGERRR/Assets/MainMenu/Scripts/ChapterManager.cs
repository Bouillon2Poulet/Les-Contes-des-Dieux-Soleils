using System;
using System.IO;
using UnityEngine;

static class ChapterManager
{
    public static int currentChapterIndex = 0;
    public static int maxChapterIndexDiscoveredByPlayer = 0;

    private static string pathToSaveFileJSON = Application.dataPath + "/Save/savefile.json";

    public enum Chapter
    {
        triton,
        eaux_divine,
        solisède,
        solimont,
        larme,
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
        public bool newGame;
    }

    public static SaveObject saveObject;

    public static void initSaveFileJSON()
    {
        // Initialize the saveObject
        saveObject = new SaveObject();
        saveObject.newGame = true;
        saveObject.currentChapter = Chapter.triton;
        saveObject.maxChapterDiscovered = Chapter.triton;

        // Convert the saveObject to JSON
        var json = JsonUtility.ToJson(saveObject);

        // Save the JSON to a file
        File.WriteAllText(pathToSaveFileJSON, json);

        Debug.Log("Save file created and saved at: " + pathToSaveFileJSON);
    }
    public static void newChapterDiscovered()
    {
        saveObject.currentChapter++;
        saveObject.maxChapterDiscovered++;
    }

    public static void getSaveFileJSONData()
    {
        if (File.Exists(pathToSaveFileJSON))
        {
            Debug.Log("FileExist!");
            // Lire le contenu du fichier JSON
            string json = File.ReadAllText(pathToSaveFileJSON);

            // Convertir le JSON en objet SaveObject
            saveObject = JsonUtility.FromJson<SaveObject>(json);

            // Mettre à jour les variables appropriées
            currentChapterIndex = (int)saveObject.currentChapter;
            maxChapterIndexDiscoveredByPlayer = (int)saveObject.maxChapterDiscovered;
        }
        else
        {
            // Si le fichier n'existe pas, initialisez-le
            initSaveFileJSON();
        }
    }
}
