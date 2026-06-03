using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private int index;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsRunning)
        {
            audioSource.clip = runSFX[index];
            PlayAudio();
        }

        if (player.IsWalking)
        {
            audioSource.clip = walkSFX[index];
            PlayAudio();
        }

        if (player.IsJumping)
        {
            audioSource.PlayOneShot(jumpSFX);
        }

        if (player.DidLand)
        {
            audioSource.PlayOneShot(landSFX);
        }

        if (player.IsWallRun)
        {
            audioSource.clip = wallRunSFX[index];
            PlayAudio();
        }

        if (player.IsSliding)
        {
            audioSource.clip = slideSFX;
            audioSource.Play();
            while (true)
            {
                audioSource.loop = true;
            }
        }
    }

    private void PlayAudio()
    {
        if (audioSource.clip = null)
        {
            return;
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
        if (index == audioSource.clip.length)
        {
            index = 0;
        }
    } 

    private void PlayNextAudio()
    {
        index++;
        audioPlaying = true;
    }
}
