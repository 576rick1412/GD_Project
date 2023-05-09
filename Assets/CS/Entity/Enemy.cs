using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Text;
using BackEnd;

public class Enemy : CharaInfo
{
    protected int enemyIndex;

    Image hpBar;                    // �� ü�¹�

    [HideInInspector]
    public Attack atk;              // ���� ����ü �迭

    [Header("�� ����")]
    float desTime = 4f;             // ��� ���� �������� �ɸ��� �ð�
    bool isDie;                     // ��� üũ
    float[] PEdis = new float[2];   // �÷��̾� - �� ������ �Ÿ� ����
    /* PEdis �ܰ�
     * 0 : ���� - �÷��̾� ����
     * 1 : �߰� - �÷��̾� �߰�
     */

    // ��� ����
    protected float boundarySpeed = 3f; // �� ��� �ӵ�
    public Transform[] wayPoints;       // �� �̵� ���
    int wayIndex;                       // �� �̵� ��� �ε���

    protected void AttackReset()
    {
        string chartId = "79721";

        Debug.Log($"{chartId}�� ��Ʈ �ҷ����⸦ ��û�մϴ�.");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}�� ��Ʈ�� �ҷ����� ��, ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log("��Ʈ �ҷ����⿡ �����߽��ϴ�. : " + bro);

        // �ε����� �̾ƿ��� ���� ����.....���� �������� üũ.......
        // ������ ���� ���� ������ ������ �����.....
        LitJson.JsonData thisEnemyData = null;
        foreach (LitJson.JsonData gameData in bro.FlattenRows())
        {
            if(int.Parse(gameData["Code"].ToString()) == enemyIndex)
            {
                thisEnemyData = gameData;
                break;
            }
        }

        atk._Damage =   int.Parse(thisEnemyData["Dmg"      ].ToString());
        atk._Delay =  float.Parse(thisEnemyData["Delay"    ].ToString());
        atk._Length = float.Parse(thisEnemyData["AtkLength"].ToString());

        PEdis[0] =    float.Parse(thisEnemyData["PEdis0"   ].ToString());
        PEdis[1] =    float.Parse(thisEnemyData["PEdis1"   ].ToString());

        _SetHP =      float.Parse(thisEnemyData["HP"       ].ToString());

        HpReset();     // ü�� �ʱ�ȭ
    }

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
        isDie = false;
        Invoke("AttackReset", 0.5f);

        Debug.Log(atk._Damage + "\n" +
            atk._Damage + "\n" +
            atk._Delay + "\n" +
            atk._Length + "\n" +
            PEdis[0] + "\n" +
            PEdis[1] + "\n" +
            _SetHP);
    }

    protected override void Update()
    {
        if (isDie)
        {
            return;
        }

        hpBar.fillAmount = _HP / _SetHP;
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
            // ���� 100% ����, �ڴ� 60% ���̷� �߻�

            Debug.DrawRay(transform.position,  transform.right       *  PEdis[i]        , Color.red);
            Debug.DrawRay(transform.position, (transform.right * -1) * (PEdis[i] * 0.6f), Color.red);

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

        // �ƹ��͵� �ش����� �ʴ´ٸ� ��� ���·� ��ȯ
        FSM(2);
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
        }
    }

    protected virtual IEnumerator EnemyAttack()
    {
        isMoveLock = true;

        setAtk = atk;
        ChangeAnim("Attack_Z");

        yield return new WaitForSeconds(setAtk._Delay);

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

        ChangeAnim(1);

        Jump();

        // ȸ��
        float dir = wayPoints[wayIndex].position.x < transform.position.x ? -1f : 1f;
        Rotate(dir);

        // ��ǥ ������ �����ߴٸ�......���� ��ǥ���� ����
        // �ƴҰ�� ��ǥ�������� �̵�
        float dis = Mathf.Abs(wayPoints[wayIndex].position.x - transform.position.x);
        if (dis <= 0.1f)
        {
            if (wayIndex + 1 == wayPoints.Length) { wayIndex = 0; }
            else { wayIndex++; }
        }
        else
        {
            transform.Translate(Vector3.right * boundarySpeed * Time.deltaTime);

            /*
            Vector2 velo = Vector2.zero;
            transform.position =
            Vector2.MoveTowards(transform.position, wayPoints[wayIndex].position, boundarySpeed * Time.deltaTime);
            */
        }

        isMoveLock = false;
        yield return null;
    }

    protected override void Jump()
    {
        float castLength = 1.4f;

        RaycastHit2D hit;
        Debug.DrawRay(transform.position, transform.right * castLength, Color.red);
        hit = Physics2D.Raycast(transform.position, transform.right, castLength, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
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

    //===============================================================
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGround = true;
            ChangeAnim("isGround", isGround);

            rigid.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGround = false;
            ChangeAnim("isGround", isGround);

            rigid.bodyType = RigidbodyType2D.Dynamic;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    //=======================< �Ⱦ��°� ��� >========================
    protected override void OnCollisionExit2D(Collision2D collision)
    {

    }
}
