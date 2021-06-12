using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Points : MonoBehaviour
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
        text.text = $"Points: {gm.points}";
    }
}
