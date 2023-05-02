using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTotem : CharaInfo
{
    Image hpBar;                    // 적 체력바

    [Header("공격 설정")]
    public Attack atk;              // 공격 구조체

    [Header("적 설정")]
    public float desTime;           // 사망 이후 삭제까지 걸리는 시간
    bool isDie;                     // 사망 체크
    public float chaseLen;          // 추격 거리

    // 물리 설정
    protected float gravityScale;   // 중력 설정

    // 추격 설정
    Transform enemyStartPos;        // 적의 처음 위치 저장

    public Transform[] wayPoints;   // 적 이동 경로
    public int wayIndex;                   // 직 이동 경로 인덱스


    protected override void Awake()
    {
        base.Awake();

        setAtk = atk;
        
        hpBar = gameObject.transform.Find("Enemy UI Canvas").
                              transform.Find("hp").
                              transform.Find("Bar").
                              gameObject.GetComponent<Image>();

        enemyStartPos = this.gameObject.transform;

        gravityScale = 10f;

        wayIndex = 0;
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
        RaycastHit2D hit;   //chaseLen

        Debug.DrawRay(transform.position, transform.right * chaseLen, Color.red);
        hit = Physics2D.Raycast(transform.position, transform.right, chaseLen);
        if(hit.collider == null)
        {
            // 무지성 왔다갔다 실행 ( 경계 )
            ChangeAnim(1);
            if (Vector2.Distance(transform.position, wayPoints[wayIndex].position) <= 0.1f)
            {
                float dir = transform.rotation.y == 0 ? -1 : 1;
                Rotate(dir);
                if (wayIndex + 1 == wayPoints.Length) { wayIndex = 0; }
                else { wayIndex++; }

                return;
            }

            Vector2 velo = Vector2.zero;
            transform.position =
            Vector2.MoveTowards(transform.position, wayPoints[wayIndex].position, _Speed * Time.deltaTime);
        }
        else if (hit.collider.gameObject.CompareTag("Player"))
        {

            Debug.DrawRay(transform.position, transform.right * atk._Length, Color.red);
            hit = Physics2D.Raycast(transform.position, transform.right, atk._Length);
            if (hit.collider == null) { }
            else if (hit.collider.gameObject.CompareTag("Player"))
            {
                StartCoroutine(EnemyAttack());
                ChangeAnim(0);
                return;
            }
            StartCoroutine(EnemyAttack());
            ChangeAnim(0);
            return;
        }
    }

    protected virtual IEnumerator EnemyAttack()
    {
        isMoveLock = true;

        ChangeAnim("Attack_Z");

        yield return new WaitForSeconds(setAtk._Delay);

        isMoveLock = false;
        yield return null;
    }

    protected virtual IEnumerator EnemyChase()
    {
        isMoveLock = true;

        float time = 0f;
        while (time <= 3)
        {
            Transform playerPos = GameObject.Find("Player").transform;
            time += Time.deltaTime;

            float dis = playerPos.position.x - transform.position.x;
            float dir = dis < 0 ? -1f : 1f;
            float rot = dis < 0 ? -1f : 0f;

            Move(dir);
            Rotate(rot);

            // 만약 플레이어와 가까워져 공격 범위 내에 들어온다면.....
            if (Vector2.Distance(playerPos.position, transform.position) < atk._Length)
            {
                goto A;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

    A:

        isMoveLock = false;
        yield return null;
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

    //=======================< 안쓰는거 비움 >========================
    protected override void OnCollisionExit2D(Collision2D collision)
    {

    }
}
