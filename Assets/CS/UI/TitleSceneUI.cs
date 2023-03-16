using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleSceneUI : MonoBehaviour
{
    public TextMeshProUGUI anyPressKey;
    private void Awake()
    {
        if (GameManager.GM.data.isKorean)
        {
            anyPressKey.text = "����Ϸ��� �ƹ�Ű�� �����ʽÿ�.";
        }
        else
        {
            anyPressKey.text = "PRESS ANY KEY TO CONTINUE.";
        }
    }

    void Update()
    {
        if (Input.anyKey == false)
        {
            return;
        }

        else
        {
            LoadingManager.LoadScene("MainScene");
        }
    }
}
