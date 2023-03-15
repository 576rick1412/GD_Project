using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.anyKey == false) return;
        else LoadingManager.LoadScene("MainScene");
    }
}
