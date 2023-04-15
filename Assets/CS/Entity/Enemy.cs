using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : CharaInfo
{
    Image hpBar;            // 적 체력바

    [Header("공격 설정")]
    public Attack[] atk;            // 공격 구조체 배열

    [Header("적 설정")]
    public float desTime;           // 사망 이후 삭제까지 걸리는 시간
    bool isDie;                     // 사망 체크
    public float nowDis;            // 적과 플레이어간의 거리
    [SerializeField]
    float[] PEdis = new float[4];   // 플레이어 - 적 사이의 거리 저장
    /* PEdis 단계
     * 0 : 공격 - 플레이어 공격
     * 1 : 추격 - 플레이어 추격
     * 2 : 경계 - 돌아다니며 플레이어 탐색
     * 3 : 포기 - 원래 지점으로 이동
     */

    // 추격 설정
    Transform enemyStartPos;        // 적의 처음 위치 저장

    
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
            // 앞은 100% 길이, 뒤는 50% 길이로 발사

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
            // 플레이어와 적 사이의 거리가 PEdis[ i ] 보다 가깝다면
            if (nowDis <= PEdis[i])
            {
                FSM(i);
                return;
            }
        }

        // 아무것도 해당하지 않는다면 애니메이션을 기본으로 바꾸고 리턴
        ChangeAnim(0);
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

            // 포기
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
