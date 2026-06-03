using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private int index;
    private bool audioPlaying;
    PlayerMovement player;
    

    [Header("Input")]
    private InputActionAsset inputActions;
    private InputAction playerMove;
    private InputAction playerSprint;
    private InputAction playerJump;
    private InputAction playerCrouch;

    [Header("Walking")]
    [SerializeField] private AudioClip[] walkSFX;

    [Header("Running")]
    [SerializeField] private AudioClip[] runSFX;

    [Header("WallRunning")]
    [SerializeField] private AudioClip[] wallRunSFX;

    [Header("Sliding")]
    [SerializeField] private AudioClip slideSFX;

    [Header("Jumping")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip landSFX;

    private void Awake()
    {
        playerMove = InputSystem.actions.FindAction("Move");
        playerJump = InputSystem.actions.FindAction("Jump");
        playerSprint = InputSystem.actions.FindAction("Sprint");
        playerCrouch = InputSystem.actions.FindAction("Crouch");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSprint.IsPressed())
        {
            Debug.Log("runSFX");
            audioSource.clip = runSFX[index];
            PlayAudio();
        }

        if (playerMove.IsPressed() && !playerSprint.IsPressed())
        {
            Debug.Log("walkSFX");
            audioSource.clip = walkSFX[index];
            PlayAudio();
        }

        if (playerJump.WasPressedThisDynamicUpdate() && player.IsJumping)
        {
            Debug.Log("jumpsfx");
            audioSource.clip = jumpSFX;
            audioSource.Play();
        }

        if (player.DidLand && playerJump.WasCompletedThisDynamicUpdate())
        {
            Debug.Log("landSFX");
            audioSource.clip = landSFX;
            audioSource.Play();
        }

        if (player.IsWallRun)
        {
            Debug.Log("walkSFX");
            audioSource.clip = wallRunSFX[index];
            PlayAudio();
        }

        if (player.IsSliding)
        {
            Debug.Log("slideSFX");
            audioSource.clip = slideSFX;
            audioSource.Play();
        }

        if (audioSource.clip = null)
        {
            return;
        }

    }

    private void PlayAudio()
    {
        index = 0;
        if (index == audioSource.clip.length)
        {
            index = 0;
        }

        if (audioPlaying)
        {
            audioSource.Play();
        }
        if (audioSource.isPlaying == false)
        {
            PlayNextAudio();
        }
        else
        {
            audioPlaying = false;
        }
        
    } 

    private void PlayNextAudio()
    {
        index++;
        audioPlaying = true;
    }
}
