using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_MunitionLeft : MonoBehaviour
{

    public TextMeshProUGUI text;
    public GameObject gun;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Munition: {gun.GetComponent<hr_RaycastWeapon>().munitionLeft}";
    }
}
