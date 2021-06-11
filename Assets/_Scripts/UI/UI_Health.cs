using UnityEngine;
using TMPro;

public class UI_Health : MonoBehaviour
{
    public TextMeshProUGUI text;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        gm = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Health: {gm.health}";
    }
}
