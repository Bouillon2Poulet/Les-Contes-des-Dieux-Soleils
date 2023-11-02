using UnityEngine;
using UnityEngine.EventSystems;

public class RECOMMENCERbtn : MonoBehaviour, IPointerClickHandler
{

    public GameObject OptionsUI;

    //Detect if a click occurs
    public void Start()
    {
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            //TODO Recommencer le chapitre
        }
    }
}
