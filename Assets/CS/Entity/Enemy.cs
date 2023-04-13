using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : CharaInfo
{
    [Header("적 설정")]
    public float desTime;           // 사망 이후 삭제까지 걸리는 시간
    bool isDie;                     // 사망 체크

    Transform PlayerPos;            // 플레이어 위치     
    public float nowDis;            // 적과 플레이어간의 거리
    [SerializeField]
    float[] PEdis = new float[4];   // 플레이어 - 적 사이의 거리 저장
    /* PEdis 단계
     * 0 : 공격 - 플레이어 공격
     * 1 : 추격 - 플레이어 추격
     * 2 : 경계 - 돌아다니며 플레이어 탐색
     * 3 : 포기 - 원래 지점으로 이동
     */

    Image hpBar;            // 적 체력바

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
            A:
            // 플레이어와 적 사이의 거리가 PEdis[ i ] 보다 가깝다면
            if (nowDis <= PEdis[i])
            {
                FSM(i);
                return;
            }
        }
    }   // 적 컨트롤 함수, 단계에 맞는 FSM 함수 실행

    void FSM(int fsmIdx)
    {
        switch (fsmIdx)
        {
            // 공격
            case 0:
                //Debug.Log("공격");
                StartCoroutine(EnemyAttack());
                break;

            // 추격
            case 1:
                //Debug.Log("추격");
                StartCoroutine(EnemyChase());
                break;

            // 경계
            case 2:
                //Debug.Log("경계");
                StartCoroutine(EnemyBoundary());
                break;

            // 포기
            case 3:
                //Debug.Log("포기");
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
