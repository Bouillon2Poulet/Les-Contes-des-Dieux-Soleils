using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarnetArrow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool isLeftArrow;
    private Carnet carnet;

    private void Start()
    {
        carnet = FindObjectOfType<Carnet>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        if (isLeftArrow)
        {
            carnet.PreviousPage();
            Debug.Log("Left Arrow");
        }
        else
        {
            carnet.NextPage();
            Debug.Log("Right Arrow");
        }
    }
}
