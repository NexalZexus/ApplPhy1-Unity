using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CamCon : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [Header("Reference")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Transform camHolder;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private Slider sensXSlider;
    [SerializeField] private Slider sensYSlider;

    private bool canLook = true;

    private float rotationX;
    private float rotationY;

    private float multiplier = 0.01f;

    private InputActionAsset InputActions;
    private InputAction lookAction;
    private Vector2 lookMouse;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        lookAction = InputSystem.actions.FindAction("Look");
    }
    // Update is called once per frame
    private void Update()
    {
        CamInput();

        camHolder.transform.localRotation = Quaternion.Euler(rotationX, rotationY, player.tilt);
        playerOrientation.transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    private void CamInput()
    {
        lookMouse = lookAction.ReadValue<Vector2>();

        rotationX -= lookMouse.y * sensY * multiplier;
        rotationY += lookMouse.x * sensX * multiplier;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
    }

    public void SetSensX()
    {
        sensX = sensXSlider.value;
    }

    public void SetSensY()
    {
        sensY = sensYSlider.value;
    }

    public void SetCanLook(bool paused)
    {
        canLook = paused;
    }
}
