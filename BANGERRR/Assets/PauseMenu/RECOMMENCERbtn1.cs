using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RECOMMENCERbtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            //TODO Recommencer le chapitre
            Debug.Log("Recommencer");
            GlobalVariables.Set("planetIndex", ChapterManager.currentChapterIndex);
            if (ChapterManager.currentChapterIndex < 6)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
