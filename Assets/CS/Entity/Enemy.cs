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
    public float nowDis;            // ���� �÷��̾�� �Ÿ�
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
        isDie = false;
    }

    protected override void Update()
    {
        if (isDie)
        {
            return;
        }

        hpBar.fillAmount = _HP / setHP;

        // �� �۵���
        EnemyControl();
    }

    void EnemyControl()
    {
        RaycastHit2D hit;
        float nowDis;

        for (int i = 0; i < PEdis.Length; i++)
        {
            // �յڷ� ���̸� ���, ���̿� �÷��̾ �浹�ߴٸ� �����ܰ�� �Ѿ
            // ���� 100% ����, �ڴ� 50% ���̷� �߻�

            Debug.DrawRay(transform.position,  transform.right       *  PEdis[i]        , Color.red);
            Debug.DrawRay(transform.position, (transform.right * -1) * (PEdis[i] * 0.5f), Color.red);

            hit = Physics2D.Raycast(transform.position, transform.right, PEdis[i], mask);
            if (hit.collider != null)
            {
                nowDis = hit.distance;
                goto A;
            }

            hit = Physics2D.Raycast(transform.position, transform.right * -1, PEdis[i] * 0.5f, mask);
            if (hit.collider != null)
            {
                nowDis = hit.distance;
                goto A;
            }

            continue;
            A:
            // �÷��̾�� �� ������ �Ÿ��� PEdis[ i ] ���� �����ٸ�
            if (nowDis <= PEdis[i])
            {
                FSM(i);
                return;
            }
        }
    }   // �� ��Ʈ�� �Լ�, �ܰ迡 �´� FSM �Լ� ����

    void FSM(int fsmIdx)
    {
        switch (fsmIdx)
        {
            // ����
            case 0:
                //Debug.Log("����");
                StartCoroutine(EnemyAttack());
                break;

            // �߰�
            case 1:
                //Debug.Log("�߰�");
                StartCoroutine(EnemyChase());
                break;

            // ���
            case 2:
                //Debug.Log("���");
                StartCoroutine(EnemyBoundary());
                break;

            // ����
            case 3:
                //Debug.Log("����");
                StartCoroutine(EnemyGiveUp());
                break;
        }
    }

    protected virtual IEnumerator EnemyAttack()
    {
        yield return null;
    }

    protected virtual IEnumerator EnemyChase()
    {
        yield return null;
    }

    protected virtual IEnumerator EnemyBoundary()
    {
        yield return null;
    }

    protected virtual IEnumerator EnemyGiveUp()
    {
        yield return null;
    }

    protected override void Die()
    {
        base.Die();

        isDie = true;

        BoxCollider2D box2D = GetComponent<BoxCollider2D>();
        box2D.enabled = false;

        Destroy(transform.Find("Enemy UI Canvas").gameObject);
        Destroy(gameObject, desTime);
    }
}
