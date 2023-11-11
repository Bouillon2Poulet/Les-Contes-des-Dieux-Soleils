using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoixBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isChoice1 = false;
    public GameObject selectionArrow;
    public Canvas ChoiceCanvas;

    private void Start()
    {
        selectionArrow.SetActive(false);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (isChoice1)
        {
            Debug.Log("Choice 1");
            EndManager.choice = 1;
            ChoiceCanvas.enabled = false;
        }
        else
        {
            Debug.Log("Choice 2");
            EndManager.choice = 2;
            ChoiceCanvas.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectionArrow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectionArrow.SetActive(false);
    }
}
