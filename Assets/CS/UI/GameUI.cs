using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI playerHpTMP;
    void Start()
    {
        
    }

    void Update()
    {
        float hh = GameObject.Find("Player").GetComponent<Player>()._HP;
        playerHpTMP.text = "Player hp : " + hh;
    }
}
