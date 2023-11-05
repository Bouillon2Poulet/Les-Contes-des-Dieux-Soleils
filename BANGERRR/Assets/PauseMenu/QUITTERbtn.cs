using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QUITTERbtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            //TODO Remettre la scene du menu principal
            Debug.Log("Quitter");
            SceneManager.LoadScene(2);
        }
    }
}
