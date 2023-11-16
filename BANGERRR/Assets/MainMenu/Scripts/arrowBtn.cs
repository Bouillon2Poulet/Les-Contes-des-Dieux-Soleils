using UnityEngine;
using UnityEngine.EventSystems;

public class arrowBtn : MonoBehaviour, IPointerClickHandler
{
    public MainMenuManager mainMenuManager;
    public bool isLeftArrow;

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            AudioManager.instance.Play("click");
            if (isLeftArrow)
            {
                mainMenuManager.GetComponent<ChapterSelectionManager>().moveLeft();
            }
            else
            {
                mainMenuManager.GetComponent<ChapterSelectionManager>().moveRight();
            }
        }
    }
}
