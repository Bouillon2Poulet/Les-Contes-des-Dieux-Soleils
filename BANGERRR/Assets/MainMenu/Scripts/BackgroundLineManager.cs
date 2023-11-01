using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition.Attributes;

public class BackgroundLineManager : MonoBehaviour
{
    public GameObject MiddleLine;
    public GameObject RightLine;
    public GameObject LeftLine;
    public GameObject BackgroundCanvas;
    private LinkedList<GameObject> Lines;
    // Start is called before the first frame update

    void Start()
    {
        Lines = new LinkedList<GameObject>();
        Lines.AddFirst(Instantiate(LeftLine, BackgroundCanvas.transform));
        Lines.Last().GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Lines.Last().SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createLine(int cameraIsMoving, int currentPlanetIndex)
    {
        if (Lines.Count() >= 2)
        {
            Debug.Log("Destroy + AddLast");
            Destroy(Lines.First());
            Lines.RemoveFirst();
        }

        if (currentPlanetIndex == 0 && cameraIsMoving == -1) //COMING BACK TO FIRST
        {
            Lines.AddLast(Instantiate(LeftLine, BackgroundCanvas.transform));
        }
        else if (currentPlanetIndex == ChapterManager.maxChapterIndexDiscoveredByPlayer && cameraIsMoving == 1)
        {
            Lines.AddLast(Instantiate(RightLine, BackgroundCanvas.transform));
        }
        else //MIDDLE
        {
            Lines.AddLast(Instantiate(MiddleLine, BackgroundCanvas.transform));
        }

        for (int i = 0; i < Lines.Count(); i++)
        {
            Debug.Log(Lines.ElementAt(i).name);
        }

        Lines.Last().GetComponent<RectTransform>().localPosition = new Vector3(cameraIsMoving * 1920, 0, 0);
        Lines.Last().SetActive(true);
    }

    public void moveLines(float cameraX, float OffSet, int cameraIsMoving)
    {
        float cameraAdvancementPercentage = (cameraX % OffSet) / OffSet;
        if (cameraAdvancementPercentage >= 0.99) cameraAdvancementPercentage = 1;
        if (cameraIsMoving < 0) cameraAdvancementPercentage = 1 - cameraAdvancementPercentage;

        // Debug.Log("Cam:" + cameraAdvancementPercentage);
        for (int i = 0; i < Lines.Count(); i++)
        {
            Debug.Log(Lines.ElementAt(i).name);
            Debug.Log(i * 1920 * cameraIsMoving + "/" + cameraAdvancementPercentage * 1920 * -cameraIsMoving);
            float PosX = i * 1920 * cameraIsMoving + cameraAdvancementPercentage * 1920 * -cameraIsMoving;
            // Debug.Log("PosX " + PosX);
            Lines.ElementAt(i).GetComponent<RectTransform>().localPosition = new Vector3(PosX, 0, 0);
        }

    }
}
