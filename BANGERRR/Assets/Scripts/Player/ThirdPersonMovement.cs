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
    public float rotationSpeed; /// default: 7
    public float moveSpeed; /// default: 7

    [Header("Drag")]
    public float playerHeight; /// mine: 1.25
    public LayerMask groundMask;
    public float raycastMargin = 0.2f; /// default: .2
    bool grounded;
    public float groundDrag; /// default: 5

    [Header("Jump")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpForce; /// default: 12
    public float jumpCooldown; /// default: .25
    public float airMultiplier; /// default 0.4
    bool readyToJump;

    /// other variables
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    
    [Header("Debug Indicator")]
    public Transform indicator;


    void Start()
    {
        LockAndHideCursor();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; /// Mandatory so that the player doesn't fall over
        gravityBody = GetComponent<GravityBody>();
        ResetJump();
    }

    void Update()
    {
        // Test (trying to aligned the calculations with the correct gravity
        //transform.up = Vector3.Slerp(transform.up, -gravityBody.GravityDirection, Time.deltaTime * rotationSpeed);

        /// GROUND CHECK
        /// Checks what's under the player using a raycast to see if the player is on the ground.
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + raycastMargin, groundMask);
        grounded = Physics.Raycast(transform.position, gravityBody.GravityDirection, playerHeight * 0.5f + raycastMargin, groundMask);
        //Debug.Log("grounded: " + grounded);

        /// ORIENTATION
        /// Gets the inputs of the joystick or keys, horizontally and vertically separately, and put them in floats.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        /// Calculates the direction in which the player if facing, only horizontally and vertically
        // Maybe all of this should be first aligned with the current gravity ?
        //Vector3 viewDirection = transform.position - new Vector3(cam.position.x, transform.position.y, cam.position.z);
        //Vector3 viewDirection = Quaternion.FromToRotation(Vector3.up, -gravityBody.GravityDirection) * (transform.position - new Vector3(cam.position.x, transform.position.y, cam.position.z));
        Vector3 gravityDirection = gravityBody.GravityDirection;
        Vector3 forwardDirection = cam.forward;
        Quaternion viewDirection = Quaternion.LookRotation(Vector3.ProjectOnPlane(forwardDirection, gravityDirection), -gravityDirection);


        /// Determines the forward vector of the sub object of the player "orientation" which is used to store its orientation
        // This makes it so that "forward" for the player is also in the direction the camera is facing (but only according to the world's plane)
        //orientation.forward = viewDirection.normalized;
        //orientation.forward = Vector3.ProjectOnPlane(viewDirection.normalized, gravityBody.GravityDirection).normalized;
        orientation.rotation = viewDirection;


        /// Determines the movement direction according to inputs and the orientation calculated above
        // How can I adapt this to the current gravity ?
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //indicator.rotation = Quaternion.FromToRotation(indicator.forward, moveDirection) * indicator.rotation;

        /// If there are inputs, we change the rotation of the player's graphics
        // Maybe here the rigidBody (which is the object that holds the player's physics) should also be turned to be aligned with the gravity?
        if (moveDirection != Vector3.zero)
        {
            //playerGraphics.forward = Vector3.Slerp(playerGraphics.forward, moveDirection.normalized, Time.deltaTime * rotationSpeed);
            //playerGraphics.up = Vector3.Slerp(playerGraphics.up, -gravityDirection, Time.deltaTime * rotationSpeed);
            //Quaternion correctRotation = Quaternion.FromToRotation(orientation.forward, moveDirection.normalized) * orientation.rotation;
            Quaternion correctRotation = Quaternion.LookRotation(moveDirection, -gravityDirection);

            //indicator.rotation = correctRotation;

            playerGraphics.rotation = Quaternion.Lerp(playerGraphics.rotation, correctRotation, rotationSpeed * Time.deltaTime);

        }

        /// DRAG
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        /// SPEED CAP
        SpeedControl();

        /// JUMP
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void FixedUpdate()
    {
        /// MOVEMENT
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Project the moveDirection onto the plane defined by the current gravity
        Vector3 moveDirectionOnGravityPlane = Vector3.ProjectOnPlane(moveDirection, gravityBody.GravityDirection).normalized;

        // Rotate the rigidBody as wanted
        Quaternion rightDirection = Quaternion.Euler(0f, moveDirectionOnGravityPlane.x * (rotationSpeed * Time.fixedDeltaTime), 0f);
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, rb.rotation * rightDirection, Time.fixedDeltaTime * 3f);

        //indicator.rotation = newRotation;
        indicator.rotation = Quaternion.FromToRotation(indicator.forward, moveDirectionOnGravityPlane) * indicator.rotation;


        rb.MoveRotation(newRotation);

        if (grounded)
        {
            //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            rb.AddForce(moveDirectionOnGravityPlane * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            //rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            rb.AddForce(moveDirectionOnGravityPlane * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        /// Here we get the flat velocity (uniqually horizontal and verticalas always, according to the world's plane)
        /// If it's too big, we limit it

        //Vector3 velocityOnGravityPlane = Vector3.ProjectOnPlane(rb.velocity, gravityBody.GravityDirection);

        //projectedVelocity.x = Mathf.Clamp(projectedVelocity.x, -moveSpeed, moveSpeed);
        //projectedVelocity.z = Mathf.Clamp(projectedVelocity.z, -moveSpeed, moveSpeed);

        //indicator.rotation = Quaternion.FromToRotation(indicator.forward, velocityOnGravityPlane) * indicator.rotation;

        //rb.velocity = Vector3.Project(projectedVelocity, rb.velocity);

        //Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 velocityOnHorizontalPlane = rb.velocity - Vector3.Dot(rb.velocity, gravityBody.GravityDirection) * gravityBody.GravityDirection;

        //Vector3 flatVel = new Vector3(velocityOnGravityPlane.x, 0f, velocityOnGravityPlane.z);
        Debug.Log(velocityOnHorizontalPlane.magnitude);
        if (velocityOnHorizontalPlane.magnitude > moveSpeed)
        {
            // Here we clamp the magnitude of velocityOnHorizontalPlane between -moveSpeed and movespeed

            Vector3 clampedHorizontalVelocity = Vector3.ClampMagnitude(velocityOnHorizontalPlane, moveSpeed);
            rb.velocity = clampedHorizontalVelocity + Vector3.Dot(rb.velocity, gravityBody.GravityDirection) * gravityBody.GravityDirection;
            //Debug.Log("limiting Velocity 8)");

            //Vector3 limitedVel = flatVel.normalized * moveSpeed;
            //rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

            //Vector3 newVel = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            //rb.velocity = Vector3.Project(newVel, rb.velocity); // À TESTER
            //rb.velocity = Vector3.Project(rb.velocity, newVel); // À TESTER
        }
    }

    private void Jump()
    {
        /// Before jumping, we reset the y velocity (again, it's only compatible for areas that are aligned with the world)
        //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Reset the velocity on the gravity plane
        Vector3 velocityOnGravityPlane = Vector3.ProjectOnPlane(rb.velocity, gravityBody.GravityDirection);
        rb.velocity = new Vector3(velocityOnGravityPlane.x, 0f, velocityOnGravityPlane.z);


        //rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // old way of jumping, adapted only to the world's plane
        rb.AddForce(-gravityBody.GravityDirection * jumpForce, ForceMode.Impulse);
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
