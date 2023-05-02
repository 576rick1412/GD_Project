using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideCard : MonoBehaviour
{
    Button button;

    public int _idx
    {
        get
        { 
            return idx; 
        }
        set 
        {
            idx = value; 
        }
    }
    int idx;

    bool isActive = true;

    GameObject blind;
    Image icon;

    void Awake()
    {
        button = GetComponent<Button>();

        blind = transform.Find("blind").gameObject;
        icon = transform.Find("Icon").gameObject.GetComponent<Image>();
    }
    void Start()
    {
        isActive = GameManager.GM.data.isEnemyConfirmeds[idx];

        blind.SetActive(!isActive);

        if (isActive)
        {
            icon.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            icon.color = new Color32(0, 0, 0, 255);
        }

        button.onClick.AddListener(() => FindObjectOfType<AllBookUI>().gameObject.
                                         GetComponent<AllBookUI>().Select_GuideCard(idx));
    }

    void Update()
    {
        isActive = GameManager.GM.data.isEnemyConfirmeds[idx];


        blind.SetActive(!isActive);
        if (isActive)
        {
            icon.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            icon.color = new Color32(0, 0, 0, 255);
        }
    }
}
