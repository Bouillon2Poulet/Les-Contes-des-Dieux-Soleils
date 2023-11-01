using UnityEngine;
using UnityEngine.EventSystems;

public class JouerBtn : MonoBehaviour, IPointerClickHandler
{
    public MainMenuManager mainMenuManager;

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            mainMenuManager.displayChapterSelectionOrLaunchGame();
        }
    }
}
