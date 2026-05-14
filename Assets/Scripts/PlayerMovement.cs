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
    private float movementMultiplier = 10f;
    private InputActionAsset inputAction;
    private InputAction playerMove;
    private Vector2 moveValue;

    [Header("Sprinting")]
    [SerializeField] private float walkSpeed = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
