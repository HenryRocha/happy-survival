using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitInteraction : MonoBehaviour, hr_IInteractable
{
    public float maxRange { get { return _maxRange; } }
    [SerializeField] private const float _maxRange = 10.0f;

    private GameManager gm;
    private GameObject tooltip;
    private BoxCollider boxCollider;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        gm = GameManager.GetInstance();
        tooltip = transform.Find("Tooltip").gameObject as GameObject;
        boxCollider = this.GetComponent<BoxCollider>();
        tooltip.SetActive(false);
    }

    public void OnStartHover()
    {
        tooltip.SetActive(true);
    }

    public void OnInteract()
    {
        gm.AddMunition();
        hr_AudioManager.instance.Play("escape");
        SceneManager.LoadScene("YouEscaped");
    }

    public void OnStopHover()
    {
        tooltip.SetActive(false);
    }
}
