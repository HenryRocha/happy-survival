using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    // Wheter the game is currently pause or not.
    public static bool GameIsPaused = false;
    public float pauseDelay = 0.5f;
    public float lastPaused = 0.5f;

    // Reference to the Pause Panel UI.
    public GameObject pausePanelUI;
    private hr_InputManager inputManager;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Get a reference to this object's hr_InputManager.
        inputManager = this.GetComponent<hr_InputManager>();
        lastPaused = Time.time;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (inputManager.pauseInput && Time.time - lastPaused > pauseDelay)
        {
            if (GameIsPaused)
            {
                Debug.Log("Paused!");
                ResumeGame();
            }
            else
            {
                Debug.Log("Continued!");
                PauseGame();
            }
            lastPaused = Time.time;
        }
    }

    public void PauseGame()
    {
        pausePanelUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        pausePanelUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitToMainMenu()
    {
        pausePanelUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}