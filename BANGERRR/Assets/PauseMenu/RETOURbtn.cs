using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RETOURbtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            AudioManager.instance.Play("click");
            PauseMenuManager.Switch();
        }
    }
}
