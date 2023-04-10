using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : CharaInfo
{
    [Header("�� ����")]
    public float desTime;   // ��� ���� �������� �ɸ��� �ð�
    Image hpBar;            // �� ü�¹�

    protected override void Awake()
    {
        base.Awake();

        hpBar = gameObject.transform.Find("Enemy UI Canvas").
                              transform.Find("HP").
                              transform.Find("Bar").
                              gameObject.GetComponent<Image>();
    }
    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        hpBar.fillAmount = _HP / setHP;
    }

    protected override void Die()
    {
        base.Die();

        BoxCollider2D box2D = GetComponent<BoxCollider2D>();

        rigid.bodyType = RigidbodyType2D.Kinematic;
        box2D.enabled = false;

        Destroy(transform.Find("Enemy UI Canvas").gameObject);
        Destroy(gameObject, desTime);
    }
}
