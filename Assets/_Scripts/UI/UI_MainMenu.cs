using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    public GameManager gm;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        gm = GameManager.GetInstance();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame()
    {
        gm.StartLevel();
        SceneManager.LoadScene("GameScene");
    }

    public void Ranking()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
