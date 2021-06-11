using UnityEngine;

public class AmmoInteraction : MonoBehaviour, hr_IInteractable
{
    public float maxRange { get { return _maxRange; } }
    [SerializeField] private const float _maxRange = 10.0f;

    private GameManager gm;
    private GameObject tooltip;
    private GameObject model1;
    private GameObject model2;
    private BoxCollider boxCollider;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        gm = GameManager.GetInstance();
        tooltip = transform.Find("Tooltip").gameObject as GameObject;
        model1 = transform.Find("SM_BoxAmmo_002").gameObject as GameObject;
        model2 = transform.Find("SM_patron_01").gameObject as GameObject;
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
        hr_AudioManager.instance.Play("ammo_pickup");

        boxCollider.enabled = false;
        model1.SetActive(false);
        model2.SetActive(false);
    }

    public void OnStopHover()
    {
        tooltip.SetActive(false);

        if (!boxCollider.enabled)
        {
            Destroy(this.gameObject);
        }
    }
}
