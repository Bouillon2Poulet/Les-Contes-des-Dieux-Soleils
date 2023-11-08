using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseOptions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject OptionsUI;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            OptionsUI.SetActive(false);
        }
    }
}
