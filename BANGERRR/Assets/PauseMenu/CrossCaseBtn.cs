using UnityEngine;
using UnityEngine.EventSystems;

public class CrossCaseBtn : MonoBehaviour, IPointerClickHandler
{

    public GameObject Cross;
    private bool isActive = false;
    //Detect if a click occurs
    public void Start()
    {
        Cross.SetActive(isActive);
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            isActive = !isActive;
            Cross.SetActive(isActive);
        }
    }
}
