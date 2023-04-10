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
    public float nowDis;                   // 적과 플레이어간의 거리
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
            // 플레이어와 적 사이의 거리가 PEdis[ i ] 보다 가깝다면
            if (nowDis <= PEdis[i])
            {
                FSM(i);
                return;
            }
        }
    }   // 적 컨트롤 함수, 단계에 맞는 FSM 함수 실행

    protected virtual void FSM(int fsmIdx)
    {
        switch (fsmIdx)
        {
            // 공격
            case 0:
                Debug.Log("공격");
                break;

            // 추격
            case 1:
                Debug.Log("추격");
                break;

            // 경계
            case 2:
                Debug.Log("경계");
                break;

            // 포기
            case 3:
                Debug.Log("포기");
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
