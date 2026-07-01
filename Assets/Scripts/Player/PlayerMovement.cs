using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform orientation;
    [SerializeField] Transform respawnPoint;

    [Header("Player Stats")]
    private float playerHeight =2f;
    private float startYScale;
    [SerializeField] private float moveSpeed = 6f;
    private float moveMult = 10f;
    [SerializeField] private float airMult = 0.3f;
    private InputActionAsset inputAction;
    private InputAction playerMove;
    private Vector2 moveValue;
    private float horiMove;    
    private float vertMove;

    [Header("Sprinting")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float acceleration = 10f;
    private InputAction playerSprint;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCount;
    private InputAction playerJump;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 4f;
    [SerializeField] private float wallDrag = 1f;
    [SerializeField] private float slideDrag = 1f;

    [Header("Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.2f;
    private bool isGrounded;

    [Header("Wall Run")]
    [SerializeField] private float wallDistance = 0.5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft = false;
    private bool wallRight = false;

    [Header("Slide")]
    [SerializeField] private float slideForce = 2f;
    [SerializeField] private float slideTimerLimit = 2f;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideYScale;
    private bool isSliding;
    private InputAction playerCrouch;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunfov;
    [SerializeField] private float wallRunfovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float tilt {  get; private set; }
    private Vector3 moveDir;
    private Vector3 slopeMoveDir;

    private Rigidbody rb;

    private RaycastHit slopeHit;

    private bool CanWallrun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight) && (Physics.Raycast(orientation.position, orientation.right, wallDistance) || Physics.Raycast(orientation.position, -orientation.right, wallDistance));
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    private void Awake()
    {
        playerMove = InputSystem.actions.FindAction("Move");
        playerJump = InputSystem.actions.FindAction("Jump");
        playerSprint = InputSystem.actions.FindAction("Sprint");
        playerCrouch = InputSystem.actions.FindAction("Crouch");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = playerTransform.localScale.y;
        slideTimer = slideTimerLimit;

    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        Sprint();
        Slide();
        CheckWall();

        if (playerJump.WasPressedThisFrame() && (isGrounded || jumpCount < 2))
        {
            Jump();
            jumpCount++; // double jump
        }

        if (isGrounded)
        {
            ResetJump(); //resets jumps
        }

        slopeMoveDir = Vector3.ProjectOnPlane(moveDir, slopeHit.normal); //for slope slipping thing

        if (CanWallrun() && !isGrounded) //wallrun
        {
            if (wallLeft)
            {
                StartWallRun();
            }
            else if (wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void MyInput()
    {
        moveValue = playerMove.ReadValue<Vector2>();
        horiMove = moveValue.x;
        vertMove = moveValue.y;

        moveDir = orientation.forward * vertMove + orientation.right * horiMove;
    }

    void Move()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDir.normalized * moveSpeed * moveMult, ForceMode.Acceleration);
        } 
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDir.normalized * moveSpeed * moveMult, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDir * moveSpeed * moveMult * airMult, ForceMode.Acceleration);
        }
    }

    void ControlDrag()
    {
        if (isGrounded && !isSliding)
        {
            rb.linearDamping = groundDrag;
        }
        else if (isGrounded && isSliding)
        {
            rb.linearDamping = slideDrag;
        }
        else 
        {
            rb.linearDamping = airDrag;
        }
    }
    
    void Sprint()
    {
        if (playerSprint.IsPressed() && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }
    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void ResetJump()
    {
        jumpCount = 1;
    }
    void Slide()
    {
        if (playerCrouch.IsPressed() && isGrounded && slideTimer > 0)
        {
            isSliding = true;
            playerTransform.localScale = new Vector3(playerTransform.localScale.x, slideYScale, playerTransform.localScale.z);
            rb.AddForce(moveDir * slideForce, ForceMode.Acceleration);
            slideTimer -= Time.deltaTime;
        }

        if (playerCrouch.WasReleasedThisFrame() || slideTimer <= 0)
        {
            isSliding = false;
            playerTransform.localScale = new Vector3(playerTransform.localScale.x, startYScale, playerTransform.localScale.z);
        }

        if (playerCrouch.WasReleasedThisFrame())
        {
            slideTimer = slideTimerLimit;
        }
    }
    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }
    void StartWallRun()
    {

        rb.linearDamping = wallDrag;

        rb.AddForce(Vector3.up * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }

        if (playerJump.WasPressedThisFrame())
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal * 2;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 50, ForceMode.Force);
                ResetJump();
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal * 2;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 50, ForceMode.Force);
                ResetJump();
            }
        }
    }
    void StopWallRun()
    {
        ControlDrag();

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);

        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
    //private void OnCollisionExit(Collision boundary)
    //{
    //    if (boundary.gameObject.tag == "Bounds")
    //    {
    //        playerTransform.position = respawnPoint.transform.position;
    //        Debug.Log("Outbounds");
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bounds")
        {
            playerTransform.position = respawnPoint.transform.position;
            Debug.Log("Outbounds");
        }
    }
}
