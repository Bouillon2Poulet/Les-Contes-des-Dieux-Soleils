using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float _speed = 6f;
    public float _turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;

    Vector3 _velocity;
    public float _gravity = -9.81f;
    public float _jumpHeight = 3f; // third person cam system
    public float _jumpForce = 1500f; // gravitybodies system

    public Transform _groundCheck;
    public float _groundDistance = 0.4f;
    public LayerMask _groundMask;

    bool _isGrounded;

    private Rigidbody _rigidbody;
    private Vector3 _direction;
    private GravityBody _gravityBody;

    void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        _gravityBody = transform.GetComponent<GravityBody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(horizontal, 0f, vertical).normalized; // normalized pour pas aller + vite si 2 touches sont activées

        // DÉPLACEMENT Y (avec la gravité et tout)
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask); // check si touche le sol
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        // DÉPLACEMENT Y (avec la gravité et tout)
        // Saut
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            //_rigidbody.AddForce(-_gravityBody.GravityDirection * _jumpForce, ForceMode.Impulse); // gravitybodies system
        }

        // Gravité
        _velocity.y += _gravity * Time.deltaTime; // Apliquer la gravité
        controller.Move(_velocity * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (_direction.magnitude >= 0.01f) // test de si ça bouge (tuto : 0.1f)
        {
            
            // On utilise Atan2 pour récupérer l'angle de direction
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // angle calculé en fonction de la direction du joueur + la rotation de la cam
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime); // pour smooth l'angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Appliquer la rotation au joueur

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // mouvement prenant en compte la rotation de la cam
            controller.Move(moveDirection.normalized * _speed * Time.deltaTime);
            

            // New way
            /*
            Vector3 direction = transform.forward * _direction.z;
            _rigidbody.MovePosition(_rigidbody.position + direction * (_speed * Time.fixedDeltaTime));

            Quaternion rightDirection = Quaternion.Euler(0f, _direction.x * (1500f * Time.fixedDeltaTime), 0f);
            Quaternion newRotation = Quaternion.Slerp(_rigidbody.rotation, _rigidbody.rotation * rightDirection, Time.fixedDeltaTime * 3f); ;
            _rigidbody.MoveRotation(newRotation);
            */
        }
    }
}
