using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : CharaInfo
{
    Image hpBar;            // �� ü�¹�

    [Header("���� ����")]
    public Attack[] atk;            // ���� ����ü �迭

    [Header("�� ����")]
    public float desTime;           // ��� ���� �������� �ɸ��� �ð�
    bool isDie;                     // ��� üũ
    public float nowDis;            // ���� �÷��̾�� �Ÿ�
    [SerializeField]
    float[] PEdis = new float[4];   // �÷��̾� - �� ������ �Ÿ� ����
    /* PEdis �ܰ�
     * 0 : ���� - �÷��̾� ����
     * 1 : �߰� - �÷��̾� �߰�
     * 2 : ��� - ���ƴٴϸ� �÷��̾� Ž��
     * 3 : ���� - ���� �������� �̵�
     */

    // �߰� ����
    Transform enemyStartPos;        // ���� ó�� ��ġ ����

    
    protected override void Awake()
    {
        base.Awake();

        hpBar = gameObject.transform.Find("Enemy UI Canvas").
                              transform.Find("HP").
                              transform.Find("Bar").
                              gameObject.GetComponent<Image>();

        enemyStartPos = this.gameObject.transform;
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
        hpBar.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        // FSM�� �������� �� �߰����� �� ��Ʈ���� ���� ����
        if (isMoveLock)
        {
            return;
        }

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

            //==================================================================

            A:
            // �÷��̾�� �� ������ �Ÿ��� PEdis[ i ] ���� �����ٸ�
            if (nowDis <= PEdis[i])
            {
                FSM(i);
                return;
            }
        }

        // �ƹ��͵� �ش����� �ʴ´ٸ� �ִϸ��̼��� �⺻���� �ٲٰ� ����
        ChangeAnim(0);
    }

    // �� ��Ʈ�� �Լ�, �ܰ迡 �´� FSM �Լ� ����
    void FSM(int fsmIdx)
    {
        switch (fsmIdx)
        {
            // ����
            case 0:
                StartCoroutine(EnemyAttack());
                ChangeAnim(0);
                break;

            // �߰�
            case 1:
                StartCoroutine(EnemyChase());
                break;

            // ���
            case 2:
                StartCoroutine(EnemyBoundary());
                break;

            // ����
            case 3:
                StartCoroutine(EnemyGiveUp());
                break;
        }
    }

    protected virtual IEnumerator EnemyAttack()
    {
        isMoveLock = true;

        setAtk = atk[0];
        ChangeAnim("Attack_Z");

        yield return new WaitForSeconds(setAtk.delay);

        isMoveLock = false;
        yield return null;
    }

    protected virtual IEnumerator EnemyChase()
    {
        isMoveLock = true;

        float time = 0f;
        while(time <= 3)
        {
            Transform playerPos = GameObject.Find("Player").transform;
            time += Time.deltaTime;

            float dis = playerPos.position.x - transform.position.x;
            float dir = dis < 0 ? -1f : 1f;
            float rot = dis < 0 ? -1f : 0f;

            Jump();
            Move(dir);
            Rotate(rot);

            // ���� �÷��̾�� ������� ���� ���� ���� ���´ٸ�.....
            if(Vector2.Distance(playerPos.position, transform.position) < PEdis[0])
            {
                goto A;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        A:

        isMoveLock = false;
        yield return null;
    }

    protected virtual IEnumerator EnemyBoundary()
    {
        isMoveLock = true;



        isMoveLock = false;
        yield return null;
    }

    protected virtual IEnumerator EnemyGiveUp()
    {
        isMoveLock = true;



        isMoveLock = false;
        yield return null;
    }

    protected override void Jump()
    {
        RaycastHit2D hit;
        float castLength = 1.4f;

        Debug.DrawRay(transform.position, transform.right * castLength, Color.red);

        hit = Physics2D.Raycast(transform.position, transform.right, castLength, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            base.Jump();
        }
    }

    protected override void Die()
    {
        base.Die();

        isDie = true;

        BoxCollider2D box2D = GetComponent<BoxCollider2D>();
        box2D.enabled = false;
        rigid.bodyType = RigidbodyType2D.Kinematic;

        Destroy(transform.Find("Enemy UI Canvas").gameObject);
        Destroy(gameObject, desTime);
    }
}
