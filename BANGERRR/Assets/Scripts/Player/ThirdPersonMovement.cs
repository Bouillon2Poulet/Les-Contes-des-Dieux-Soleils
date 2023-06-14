using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Imports")]
    public Transform orientation;
    public Transform playerGraphics;
    public Transform cam;
    private Rigidbody rb;
    private GravityBody gravityBody;

    [Header("Player Variables")]
    public float rotationSpeed; // default: 7
    public float moveSpeed; // default: 7

    [Header("Drag")]
    public float playerHeight; // mine: 1.25
    public LayerMask groundMask;
    public float raycastMargin = 0.2f; // default: .2
    bool grounded;
    public float groundDrag; // default: 5

    [Header("Jump")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpForce; // default: 12
    public float jumpCooldown; // default: .25
    public float airMultiplier; // default 0.4
    bool readyToJump;

    // other variables
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;


    void Start()
    {
        LockAndHideCursor();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Mandatory so that the player doesn't fall over
        gravityBody = GetComponent<GravityBody>();
        ResetJump();
    }

    void Update()
    {
        // GROUND CHECK
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + raycastMargin, groundMask);
        Debug.Log("grounded: " + grounded);

        // ORIENTATION
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 viewDirection = transform.position - new Vector3(cam.position.x, transform.position.y, cam.position.z);

        orientation.forward = viewDirection.normalized;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (moveDirection != Vector3.zero)
        {
            playerGraphics.forward = Vector3.Slerp(playerGraphics.forward, moveDirection.normalized, Time.deltaTime * rotationSpeed);
        }

        // DRAG
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // SPEED CAP
        SpeedControl();

        // JUMP
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void FixedUpdate()
    {
        // MOVEMENT
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
