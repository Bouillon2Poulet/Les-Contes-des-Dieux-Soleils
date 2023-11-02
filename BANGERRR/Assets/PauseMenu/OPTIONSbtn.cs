using UnityEngine;
using UnityEngine.EventSystems;

public class OPTIONSbtn : MonoBehaviour, IPointerClickHandler
{

    public GameObject OptionsUI;

    //Detect if a click occurs
    public void Start()
    {
        OptionsUI.SetActive(false);
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            OptionsUI.SetActive(true);
        }
    }
}
