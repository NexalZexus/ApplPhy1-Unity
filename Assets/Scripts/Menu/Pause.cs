using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private InputActionAsset inputActions;
    private InputAction playerPause;

    private CamCon camLock;

    private bool paused = false;

    [SerializeField] GameObject menu;

    private void Awake()
    {
        playerPause = InputSystem.actions.FindAction("Pause");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        menu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerPause.WasPressedThisFrame())
        {
            camLock.SetCanLook(paused);
            if (paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }
    
    private void Stop()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Play()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Quit()
    {
        SceneManager.LoadScene(1);
        Cursor.lockState = CursorLockMode.None;
    }
}
