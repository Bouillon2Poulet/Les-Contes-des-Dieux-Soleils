using UnityEngine;
using UnityEngine.EventSystems;

public class CrossCaseBtn : MonoBehaviour, IPointerClickHandler
{
    public GameObject Cross;
    private bool isActive = false;

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
            DialogManager.instance.DialoguesRapides(isActive);
            Debug.Log("Dialogues rapides : " + isActive);
        }
    }
}
