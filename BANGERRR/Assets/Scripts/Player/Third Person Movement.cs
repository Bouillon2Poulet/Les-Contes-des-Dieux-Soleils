using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        // normalized pour pas aller + vite si 2 touches sont activées

        if (direction.magnitude >= 0.01f) // test de si ça bouge (tuto : 0.1f)
        {
            // On utilise Atan2 pour récupérer l'angle de direcrtion
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // angle calculé en fonction de la direction du joueur + la rotation de la cam
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // pour smooth l'angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Appliquer la rotation au joueur

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // mouvement prenant en compte la rotation de la cam
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
    }
}
