using UnityEngine;
using UnityEngine.EventSystems;

public class OPTIONSbtn : MonoBehaviour, IPointerClickHandler
{
    public GameObject OptionsUI;

    public void Start()
    {
        OptionsUI.SetActive(false);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        AudioManager.instance.Play("click");
        Debug.Log("OptionBtn");
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            OptionsUI.SetActive(true);
        }
    }
}
