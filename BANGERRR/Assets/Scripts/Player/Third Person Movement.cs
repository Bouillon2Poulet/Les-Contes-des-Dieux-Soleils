using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    // DÉPLACEMENT X - Y
    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // DÉPLACEMENT Y (avec la gravité et tout)
    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // DÉPLACEMENT Y (avec la gravité et tout)
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // check si touche le sol
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        // normalized pour pas aller + vite si 2 touches sont activées

        // DÉPLACEMENT X - Y
        if (direction.magnitude >= 0.01f) // test de si ça bouge (tuto : 0.1f)
        {
            // On utilise Atan2 pour récupérer l'angle de direcrtion
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // angle calculé en fonction de la direction du joueur + la rotation de la cam
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // pour smooth l'angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Appliquer la rotation au joueur

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // mouvement prenant en compte la rotation de la cam
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        // DÉPLACEMENT Y (avec la gravité et tout)
        // Saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravité
        velocity.y += gravity * Time.deltaTime; // Apliquer la gravité
        controller.Move(velocity * Time.deltaTime);
    }
}
