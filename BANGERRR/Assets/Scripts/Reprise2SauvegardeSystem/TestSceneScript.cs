using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneScript : MonoBehaviour
{
    public int planetIndexFromSave = 0;

    void Awake()
    {
        GlobalVariables.Set("planetIndex", planetIndexFromSave);

        SceneManager.LoadScene(0);
    }
}
