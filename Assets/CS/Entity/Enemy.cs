using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Text;
using BackEnd;

public class Enemy : CharaInfo
{
    protected int enemyIndex;

    Image hpBar;                    // 적 체력바

    [HideInInspector]
    public Attack atk;              // 공격 구조체 배열

    [Header("적 설정")]
    float desTime = 4f;             // 사망 이후 삭제까지 걸리는 시간
    bool isDie;                     // 사망 체크
    float[] PEdis = new float[2];   // 플레이어 - 적 사이의 거리 저장
    /* PEdis 단계
     * 0 : 공격 - 플레이어 공격
     * 1 : 추격 - 플레이어 추격
     */

    // 경계 설정
    protected float boundarySpeed = 3f; // 적 경계 속도
    public Transform[] wayPoints;       // 적 이동 경로
    int wayIndex;                       // 적 이동 경로 인덱스

    protected void AttackReset()
    {
        string chartId = "79721";

        Debug.Log($"{chartId}의 차트 불러오기를 요청합니다.");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}의 차트를 불러오는 중, 에러가 발생했습니다. : " + bro);
            return;
        }

        Debug.Log("차트 불러오기에 성공했습니다. : " + bro);

        // 인덱스로 뽑아오는 법을 몰라.....직접 돌려보며 체크.......
        // 아이템 수가 적기 때문에 가능한 방법인.....
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

        HpReset();     // 체력 초기화
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

        // FSM이 실행중일 시 추가적인 적 컨트롤을 하지 않음
        if (isMoveLock)
        {
            return;
        }

        // 적 작동부
        EnemyControl();
    }

    void EnemyControl()
    {
        RaycastHit2D hit;
        float nowDis;

        for (int i = 0; i < PEdis.Length; i++)
        {
            // 앞뒤로 레이를 쏘고, 레이에 플레이어가 충돌했다면 다음단계로 넘어감
            // 앞은 100% 길이, 뒤는 60% 길이로 발사

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
            // 플레이어와 적 사이의 거리가 PEdis[ i ] 보다 가깝다면
            if (nowDis <= PEdis[i])
            {
                FSM(i);
                return;
            }
        }

        // 아무것도 해당하지 않는다면 경계 상태로 전환
        FSM(2);
    }

    // 적 컨트롤 함수, 단계에 맞는 FSM 함수 실행
    void FSM(int fsmIdx)
    {
        switch (fsmIdx)
        {
            // 공격
            case 0:
                StartCoroutine(EnemyAttack());
                ChangeAnim(0);
                break;

            // 추격
            case 1:
                StartCoroutine(EnemyChase());
                break;

            // 경계
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

            // 만약 플레이어와 가까워져 공격 범위 내에 들어온다면.....
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

        // 회전
        float dir = wayPoints[wayIndex].position.x < transform.position.x ? -1f : 1f;
        Rotate(dir);

        // 목표 지점에 도달했다면......다음 목표지점 지정
        // 아닐경우 목표지점으로 이동
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

    //=======================< 안쓰는거 비움 >========================
    protected override void OnCollisionExit2D(Collision2D collision)
    {

    }
}
