using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform destination;
    public float moveTime = 5.0f;
    private float elapsedTime = 0.0f;
    private Vector3 initialPosition;
    public bool canMove = false;

    private bool hasReachedFinalPosition = false;
    public GameObject jouerBtnFromSelection;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (canMove && GetComponentInParent<MainMenuManager>().step == 0 && !hasReachedFinalPosition)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < moveTime)
            {
                float t = elapsedTime / moveTime;
                transform.position = Vector3.Lerp(initialPosition, destination.position, t);
            }
            else
            {
                hasReachedFinalPosition = true;

                transform.position = destination.position;
                GetComponentInParent<MainMenuManager>().step = 1;

                jouerBtnFromSelection.SetActive(true);
            }
        }
    }
}
