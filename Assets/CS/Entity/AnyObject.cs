using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyObject : MonoBehaviour
{
    bool isActivateCheck;   // Ȱ��ȭ üũ
    GameObject keyObject;   // ����Ű ǥ�� ������Ʈ
    public KeyCode kbhit;   // ����Ű

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
        Debug.Log("����!!");
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
