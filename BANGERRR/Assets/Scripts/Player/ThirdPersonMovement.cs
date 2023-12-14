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
    public Animator animator;

    [Header("Player Variables")]
    public float rotationSpeed; /// default: 7
    public float moveSpeed; /// default: 7
    public bool isListeningToMoveInputs = true;
    public bool isFollowingGA = true;
    private bool isSpeedCaped = true;

    [Header("Drag")]
    public float playerHeight; /// mine: 1.25
    public LayerMask groundMask;
    [Range(-.5f, .5f)] public float raycastMargin = .2f; /// default: .2
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

    [Header("Sounds")]
    private int walkSoundsTimer = 1;

    [Header("Gravity Stuff")]
    //public Transform gravityAreaTransform; // this debug feature allows me to retrieve the current gravityArea the player is in
    private Vector3 GAPreviousPosition;
    float GAPreviousRotationY;
    private bool GAFirstEntering;
    private int GAPreviousID;

    [Header("JETPACK MODE")]
    public bool JETPACKMODE;
    [SerializeField] GameObject Jetpack_left;
    [SerializeField] GameObject Jetpack_right;
    [SerializeField] GameObject Jetpack_front;
    [SerializeField] GameObject Jetpack_back;
    [SerializeField] public ParticleSystem[] pSys;
    private ParticleSystem.EmissionModule pSysEmMod;

    [Header("Bulle Jetpack")]
    public static bool wearingBulleJetpack = false;
    [SerializeField] GameObject BulleJetpack;
    [SerializeField] GameObject BulleJetpackSide;
    bool isJetpackSoundPlaying = false;

    private enum Direction { right, left, front, back, };
    Direction facing;

    GameObject sphere;
    bool canCheckIfGrounded = true;

    void Start()
    {
        LockAndHideCursor();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; /// Mandatory so that the player doesn't fall over
        gravityBody = GetComponent<GravityBody>();
        ResetJump();

        GAPreviousPosition = Vector3.zero;
        GAPreviousRotationY = 0;
        GAFirstEntering = true;
        GAPreviousID = -1;

        foreach (ParticleSystem psys in pSys)
        {
            psys.Play();
            ToggleEmission(psys, false);
        }

        // DEBUG TO DELETE
        /*sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.SetParent(rb.transform);
        sphere.transform.localScale = new Vector3(.25f, .25f, .25f);
        sphere.transform.localPosition = new Vector3(0f, 0f, 0f);
        sphere.transform.SetParent(null);
        sphere.GetComponent<SphereCollider>().isTrigger = true;*/
    }

    void Update()
    {
        //DEBUG TO DELETE
        /*Vector3 ppp = new Vector3(0f, -(playerHeight * 0.5f + raycastMargin), 0f);
        sphere.transform.position = rb.transform.TransformPoint(ppp);*/

        /// GROUND CHECK
        /// Checks what's under the player using a raycast to see if the player is on the ground.
        //grounded = Physics.Raycast(transform.position, gravityBody.GravityDirection, playerHeight * 0.5f + raycastMargin, groundMask);
        grounded = Physics.Raycast(transform.position, gravityBody.GravityDirection, out RaycastHit hit, playerHeight * 0.5f + raycastMargin);
        grounded = grounded && (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Planet"));


        /// ORIENTATION - Gets the inputs of the joystick or keys, horizontally and vertically separately, and put them in floats.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        IndicateDirectionToAnimator();

        /// Calculates the direction in which the player if facing
        Vector3 gravityDirection = gravityBody.GravityDirection;
        Vector3 forwardDirection = cam.forward;
        Quaternion viewDirection = Quaternion.LookRotation(Vector3.ProjectOnPlane(forwardDirection, gravityDirection), -gravityDirection);

        /// Determines the rotation of the child object (called "orientation") of the player responsible for the orientation
        orientation.rotation = Quaternion.Lerp(orientation.rotation, viewDirection, rotationSpeed * Time.deltaTime);

        /// Determines the movement direction according to inputs and the orientation calculated above
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        /// If there are inputs, we change the rotation of the player's graphics
        if (moveDirection != Vector3.zero)
        {
            Quaternion correctRotation = Quaternion.LookRotation(moveDirection, -gravityDirection);
            playerGraphics.rotation = Quaternion.Lerp(playerGraphics.rotation, correctRotation, rotationSpeed * Time.deltaTime);
        }

        /// DRAG - Drag is applied on the rigidbody of the player only if they're an the ground
        if (grounded || JETPACKMODE)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        /// SPEED CAP - This function limits the velocity of the player
        if (isSpeedCaped)
        {
            SpeedControl();
        }

        /// JUMP
        if (Input.GetKey(jumpKey) && readyToJump && grounded && isListeningToMoveInputs)
        {
            readyToJump = false;
            animator.SetBool("Jumping", true);
            animator.SetBool("Inair", false);
            //Debug.Log("JUMP!!");
            canCheckIfGrounded = false;
            Jump();
            SaveLastJumpPosition();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void FixedUpdate()
    {
        if (wearingBulleJetpack)
        {
            BulleJetpack.SetActive(facing == Direction.front || facing == Direction.back);
            BulleJetpackSide.SetActive(facing == Direction.right || facing == Direction.left);
        }

        if (canCheckIfGrounded)
        {
            if (grounded)
            {
                animator.SetBool("Jumping", false);
                animator.SetBool("Inair", false);
                //Debug.Log("NO JUMP, ONGROUND");
            }
            else
            {
                animator.SetBool("Inair", true);
                animator.SetBool("Jumping", false);
                //Debug.Log("INAIR");
            }
        }

        if (gravityBody.GravityTransform != null && isFollowingGA)
        {
            FollowGravityArea();
            if (isListeningToMoveInputs)
            {
                MovePlayer();
            }
            if (JETPACKMODE)
            {
                // NULLIFY GRAVITY (A BIT)
                rb.AddForce(-gravityBody.GravityDirection * (gravityBody.GravityForce * Time.fixedDeltaTime * .2f), ForceMode.Acceleration);

                // JETPACK INPUTS
                // UP
                if (Input.GetKey(KeyCode.E))
                {
                    rb.AddForce(-gravityBody.GravityDirection * (gravityBody.GravityForce * Time.fixedDeltaTime * 1.8f), ForceMode.Force);

                    ToggleEmission(pSys[0], facing == Direction.left);
                    ToggleEmission(pSys[1], facing == Direction.right);
                    ToggleEmission(pSys[2], facing == Direction.front || facing == Direction.back);
                    ToggleEmission(pSys[3], facing == Direction.front || facing == Direction.back);
                }
                else
                {
                    foreach(ParticleSystem psys in pSys)
                    {
                        ToggleEmission(psys, false);
                    }
                    Debug.Log("NOT GOING UP");
                }

                // DOWN
                if (Input.GetKey(KeyCode.Q))
                {
                    rb.AddForce(gravityBody.GravityDirection * (gravityBody.GravityForce * Time.fixedDeltaTime * 3f), ForceMode.Force);
                    if (!isJetpackSoundPlaying)
                    {
                        AudioManager.instance.Play("thruster");
                        isJetpackSoundPlaying = true;
                    }
                }
                else
                {
                    if (isJetpackSoundPlaying)
                    {
                        AudioManager.instance.Stop("thruster");
                        isJetpackSoundPlaying = false;
                    }
                }

                Jetpack_left.SetActive(facing == Direction.left);
                Jetpack_right.SetActive(facing == Direction.right);
                Jetpack_front.SetActive(facing == Direction.front);
                Jetpack_back.SetActive(facing == Direction.back);
            }
            else
            {
                if (isJetpackSoundPlaying)
                {
                    AudioManager.instance.Stop("thruster");
                    isJetpackSoundPlaying = false;
                }
                Jetpack_left.SetActive(false);
                Jetpack_right.SetActive(false);
                Jetpack_front.SetActive(false);
                Jetpack_back.SetActive(false);
            }
        }
        else
        {
            GAFirstEntering = true;
        }
    }

    private void FollowGravityArea()
    {
        int currentID = gravityBody.GravityTransform.GetInstanceID();

        if (currentID != GAPreviousID || GAFirstEntering)
        {
            GAPreviousID = currentID;
            GAPreviousPosition = gravityBody.GravityTransform.position;
            GAPreviousRotationY = gravityBody.GravityTransform.rotation.eulerAngles.y;
            GAFirstEntering = false;
        }
        else
        {
            // Follow GA position
            Vector3 GAPositionDelta = gravityBody.GravityTransform.position - GAPreviousPosition;
            //rb.MovePosition(rb.position - GAPositionDelta); // old way
            rb.position += GAPositionDelta;

            // Follow GA rotation
            Vector3 offset = rb.position - GAPreviousPosition;
            
            float deltaRotation = gravityBody.GravityTransform.rotation.eulerAngles.y - GAPreviousRotationY;
            Quaternion rotation = Quaternion.Euler(0f, deltaRotation, 0f);
            Vector3 rotatedOffset = rotation * offset;

            rb.position = GAPreviousPosition + rotatedOffset;

            // Store for next time
            GAPreviousRotationY = gravityBody.GravityTransform.rotation.eulerAngles.y;
            GAPreviousPosition = gravityBody.GravityTransform.position;
        }
    }

    public void TeleportPlayerTo(Vector3 position)
    {
        rb.position = position;
    }

    private void MovePlayer()
    {
        /// Project the moveDirection vector onto the plane defined by the current gravity
        Vector3 moveDirectionOnGravityPlane = Vector3.ProjectOnPlane(moveDirection, gravityBody.GravityDirection).normalized;

        /// Rotate the rigidBody as wanted
        Quaternion rightDirection = Quaternion.Euler(0f, moveDirectionOnGravityPlane.x * (rotationSpeed * Time.fixedDeltaTime), 0f);
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, rb.rotation * rightDirection, Time.fixedDeltaTime * 3f);
        rb.MoveRotation(newRotation);

        /// Apply the movement forces to the player's rigidbody
        if (grounded)
        {
            rb.AddForce(moveDirectionOnGravityPlane * moveSpeed * 10f, ForceMode.Force);

            // WALK SOUND
            if (moveDirectionOnGravityPlane.sqrMagnitude > 0.98)
            {
                walkSoundsTimer--;
                if (walkSoundsTimer == 0)
                {
                    AudioManager.instance.Play("walk" + Random.Range(1, 8));
                    walkSoundsTimer = 21;
                }
            }
            else
            {
                walkSoundsTimer = 1;
            }
        }
        else if (JETPACKMODE)
        {
            rb.AddForce(moveDirectionOnGravityPlane * moveSpeed * 50f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirectionOnGravityPlane * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            walkSoundsTimer = 1;
        }
    }

    private void SpeedControl()
    {
        /// Calculation of the lateral velocity of the player on the plan defined by the gravity
        Vector3 velocityOnHorizontalPlane = rb.velocity - Vector3.Dot(rb.velocity, gravityBody.GravityDirection) * gravityBody.GravityDirection;

        /// If the velocity is too big, we clamp it
        if (velocityOnHorizontalPlane.magnitude > moveSpeed)
        {
            Vector3 clampedHorizontalVelocity = Vector3.ClampMagnitude(velocityOnHorizontalPlane, moveSpeed);
            rb.velocity = clampedHorizontalVelocity + Vector3.Dot(rb.velocity, gravityBody.GravityDirection) * gravityBody.GravityDirection;
        }
    }

    private void Jump()
    {
        /// Giving only the lateral velocity to the rb to cancel up and down velocity
        Vector3 velocityOnGravityPlane = Vector3.ProjectOnPlane(rb.velocity, gravityBody.GravityDirection);
        rb.velocity = velocityOnGravityPlane;

        AudioManager.instance.Play("jump" + Random.Range(1, 6));

        rb.AddForce(-gravityBody.GravityDirection * jumpForce, ForceMode.Impulse);
    }

    public void Eject(float force)
    {
        Vector3 velocityOnGravityPlane = Vector3.ProjectOnPlane(rb.velocity, gravityBody.GravityDirection);
        rb.velocity = velocityOnGravityPlane;

        rb.AddForce(-gravityBody.GravityDirection * force, ForceMode.Impulse);
    }

    public void SaveLastJumpPosition()
    {
        LastJumpPosition.instance.transform.position = rb.position;
    }

    private void ResetJump()
    {
        readyToJump = true;
        canCheckIfGrounded = true;
    }

    //char myDirection = '.';

    private void IndicateDirectionToAnimator()
    {
        if (isListeningToMoveInputs)
        {
            animator.SetBool("Walking", false);

            if (verticalInput > .01f)
            {
                animator.SetBool("GoingBack", true);
                animator.SetBool("GoingRight", false);
                animator.SetBool("GoingFace", false);
                animator.SetBool("GoingLeft", false);
                facing = Direction.back;
                animator.SetBool("Walking", true);
            }
            else if (verticalInput < -.01f)
            {
                animator.SetBool("GoingFace", true);
                animator.SetBool("GoingBack", false);
                animator.SetBool("GoingRight", false);
                animator.SetBool("GoingLeft", false);
                facing = Direction.front;
                animator.SetBool("Walking", true);
            }
            else if (horizontalInput > .01f)
            {
                animator.SetBool("GoingRight", true);
                animator.SetBool("GoingBack", false);
                animator.SetBool("GoingFace", false);
                animator.SetBool("GoingLeft", false);
                facing = Direction.right;
                animator.SetBool("Walking", true);
            }
            else if (horizontalInput < -.01f)
            {
                animator.SetBool("GoingLeft", true);
                animator.SetBool("GoingBack", false);
                animator.SetBool("GoingRight", false);
                animator.SetBool("GoingFace", false);
                facing = Direction.left;
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void blockPlayerMoveInputs()
    {
        isListeningToMoveInputs = false;
        animator.SetBool("Jumping", false);
        animator.SetBool("Inair", false);
    }

    public void unblockPlayerMoveInputs()
    {
        isListeningToMoveInputs = true;
    }

    public void blockPlayerGAFollow()
    {
        isFollowingGA = false;
        gravityBody.SetForceApplication(false);
    }

    public void unblockPlayerGAFollow()
    {
        isFollowingGA = true;
        gravityBody.SetForceApplication(true);
    }

    public void UncapSpeed()
    {
        isSpeedCaped = false;
    }

    public void CapSpeed()
    {
        isSpeedCaped = true;
    }

    public void ToggleEmission(ParticleSystem pSys, bool state)
    {
        var em = pSys.emission;
        em.enabled = state;
    }

    public static void ToggleBulleJetpack(bool state)
    {
        wearingBulleJetpack = state;
    }
}
