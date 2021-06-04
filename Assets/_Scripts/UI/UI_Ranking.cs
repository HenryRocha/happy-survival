using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Ranking : MonoBehaviour
{
    private GameManager gm;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        text = gameObject.GetComponent<TextMeshProUGUI>();
        string msg = "";
        for (int i = 0; i<10; i++) {
            msg += $"{gm.highScorePlayers[i]}: {gm.highScores[i]}\n";
        }
        text.text = msg;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
