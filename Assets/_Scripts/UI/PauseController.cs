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

    public void DeterminePause()
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