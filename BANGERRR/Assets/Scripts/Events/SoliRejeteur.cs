using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliRejeteur : MonoBehaviour
{
    [Header("Rejection")]
    public Rigidbody player;
    public float initialRejectionForce = 10f;
    [Header("Rotation des lézards")]
    public Transform Lezard1;
    public Transform Lezard2;
    public NPC text;
    public float rotationSpeed = 2f;

    private bool isPlayerIn = false;
    private float angle = 0f;
    private bool rejectionTriggered = false;
    private float rejectionForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerIn = false;
            ResetRejection();
        }
    }

    private void FixedUpdate()
    {
        if (isPlayerIn)
        {
            if (player.velocity.magnitude > 6.5 && !rejectionTriggered)
            {
                TriggerRejection();
            }

            if (rejectionTriggered)
            {
                Vector3 rejectionVector = transform.position - player.position;

                player.AddForce(-rejectionVector * rejectionForce, ForceMode.Force);
                rejectionForce += 1;

                angle += Time.fixedDeltaTime * rotationSpeed;
                Lezard1.localRotation = Quaternion.Euler(0f, angle, 0f);
                Lezard2.localRotation = Quaternion.Euler(0f, angle, 0f);
            }
        }
    }

    public void TriggerRejection()
    {
        rejectionTriggered = true;
        FindObjectOfType<DialogManager>().OpenDialog(text.messagesB, text.actors);
        Debug.Log("REJECTION TRIGGERED!");
    }

    public void ResetRejection()
    {
        rejectionTriggered = false;
        rejectionForce = initialRejectionForce;
        Debug.Log("Rejection reset.");
    }

    private void Start()
    {
        rejectionForce = initialRejectionForce;
    }
}
