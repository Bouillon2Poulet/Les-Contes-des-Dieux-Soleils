using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform destination;
    public float moveTime = 5.0f;
    private float elapsedTime = 0.0f;
    private Vector3 initialPosition;
    public bool canMove = false;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (canMove && GetComponentInParent<MainMenuManager>().step == 0)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < moveTime)
            {
                float t = elapsedTime / moveTime;
                transform.position = Vector3.Lerp(initialPosition, destination.position, t);
            }
            else
            {
                // Arriver à la destination après 5 secondes
                transform.position = destination.position;
                GetComponentInParent<MainMenuManager>().step = 1;
            }
        }
    }
}
