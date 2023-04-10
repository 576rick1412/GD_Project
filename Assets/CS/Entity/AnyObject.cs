using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyObject : MonoBehaviour
{
    bool isActivateCheck;   // 활성화 체크
    GameObject keyObject;   // 조작키 표시 오브젝트
    public KeyCode kbhit;   // 조작키

    protected virtual void Start()
    {
        isActivateCheck = false;

        keyObject = gameObject.transform.Find("keyObject").gameObject;
        keyObject.SetActive(isActivateCheck);
    }

    protected virtual void Update()
    {
        keyObject.SetActive(isActivateCheck);

        if (Input.anyKey == false)
        {
            return;
        }

        if(Input.GetKeyDown(kbhit) && isActivateCheck)
        {
            ObjectEvent();
        }
    }

    protected virtual void ObjectEvent()
    {
        Debug.Log("누름!!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isActivateCheck = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isActivateCheck = false;
        }
    }
}
