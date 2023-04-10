using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : CharaInfo
{
    [Header("�� ����")]
    public float desTime;           // ��� ���� �������� �ɸ��� �ð�
    bool isDie;                     // ��� üũ

    Transform PlayerPos;            // �÷��̾� ��ġ     
    public float nowDis;                   // ���� �÷��̾�� �Ÿ�
    [SerializeField]
    float[] PEdis = new float[4];   // �÷��̾� - �� ������ �Ÿ� ����
    /* PEdis �ܰ�
     * 0 : ���� - �÷��̾� ����
     * 1 : �߰� - �÷��̾� �߰�
     * 2 : ��� - ���ƴٴϸ� �÷��̾� Ž��
     * 3 : ���� - ���� �������� �̵�
     */

    Image hpBar;            // �� ü�¹�

    protected override void Awake()
    {
        base.Awake();

        hpBar = gameObject.transform.Find("Enemy UI Canvas").
                              transform.Find("HP").
                              transform.Find("Bar").
                              gameObject.GetComponent<Image>();

        PlayerPos = GameObject.Find("Player").transform;
    }
    protected override void Start()
    {

    }

    protected override void Update()
    {
        if (isDie)
        {
            return;
        }
        hpBar.fillAmount = _HP / setHP;

        EnemyControl();
    }


    void EnemyControl()
    {
        nowDis = Vector2.Distance(PlayerPos.position, gameObject.transform.position);

        for (int i = 0; i < PEdis.Length; i++)
        {
            // �÷��̾�� �� ������ �Ÿ��� PEdis[ i ] ���� �����ٸ�
            if (nowDis <= PEdis[i])
            {
                FSM(i);
                return;
            }
        }
    }   // �� ��Ʈ�� �Լ�, �ܰ迡 �´� FSM �Լ� ����

    protected virtual void FSM(int fsmIdx)
    {
        switch (fsmIdx)
        {
            // ����
            case 0:
                Debug.Log("����");
                break;

            // �߰�
            case 1:
                Debug.Log("�߰�");
                break;

            // ���
            case 2:
                Debug.Log("���");
                break;

            // ����
            case 3:
                Debug.Log("����");
                break;
        }
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
