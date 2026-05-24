using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform orientation;

    [Header("Player Stats")]
    private float playerHeight =2f;
    private float startYScale;
    [SerializeField] private float moveSpeed = 6f;
    private float moveMult = 10f;
    [SerializeField] private float airMult = 0.3f;
    private InputActionAsset inputAction;
    private InputAction playerMove;
    private Vector2 moveValue;

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

    [Header("Slide")]
    [SerializeField] private float slideForce = 2f;
    [SerializeField] private float slideTimerLimit = 2f;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideYScale;
    private bool isSliding;
    private InputAction playerCrouch;

    private float horiMove;
    private float vertMove;

    public float tilt {  get; private set; }

    private Vector3 moveDir;

    private Rigidbody rb;

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
        
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();
        if (playerJump.WasPressedThisFrame() && (isGrounded || jumpCount < 2))
        {
            Jump();
            jumpCount++;
        }

        if (isGrounded)
        {
            jumpCount = 1;
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
        if (isGrounded)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * moveMult, ForceMode.Acceleration);
        } 
        else if (!isGrounded)
        {
            rb.AddForce(moveDir * moveSpeed * moveMult * airMult, ForceMode.Acceleration);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        } 
        else 
        {
            rb.linearDamping = airDrag;
        }
    }
    
    void ControlSpeed()
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
}
