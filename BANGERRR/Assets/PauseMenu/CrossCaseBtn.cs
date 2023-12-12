using UnityEngine;
using UnityEngine.EventSystems;

public class CrossCaseBtn : MonoBehaviour, IPointerClickHandler
{
    public GameObject Cross;
    private bool isActive = false;

    public void Start()
    {
        Cross.SetActive(PlayerPrefs.GetInt("dialoguesRapides") == 1);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        AudioManager.instance.Play("click");
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            isActive = !isActive;
            Cross.SetActive(isActive);
            DialogManager.instance.DialoguesRapides(isActive);
            PlayerPrefs.SetInt("dialoguesRapides", isActive ? 1 : 0);
            //Debug.Log("Dialogues rapides : " + isActive);
            //Debug.Log("Dialogues rapides : " + (PlayerPrefs.GetInt("dialoguesRapides") == 1));
        }
    }
}
