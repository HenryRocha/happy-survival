using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelInteractable : MonoBehaviour, IInteractable
{
    private GameManager gm;
    public float maxRange { get { return _maxRange; } }

    [SerializeField] private const float _maxRange = 3.0f;

    private GameObject tooltip;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        gm = GameManager.GetInstance();
        tooltip = transform.Find("Tooltip").gameObject as GameObject;
        tooltip.SetActive(false);
    }

    public void OnStartHover()
    {
        Debug.Log("Ready to interact.");
        tooltip.SetActive(true);
    }

    public void OnInteract()
    {
        Debug.Log("Interacted!");
        gm.Reset();
        SceneManager.LoadScene("MainMenu");
    }

    public void OnStopHover()
    {
        Debug.Log("Interaction stopped.");
        tooltip.SetActive(false);
    }
}
