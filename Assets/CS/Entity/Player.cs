using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaInfo
{
    [Header("조작키 설정")]
    public KeyCodeManager KCM;  // 조작키 매니저

    [Header("투척검")]
    public Knife knife;

    [Header("플레이어 설정")]
    public bool isJumpCool;     // 점프 가능 상태, 거짓일 때 점프 가능
    public float jumpDelay;     // 점프 딜레이

    public Attack atk;
    public Attack skill_1;

    float setH = 1;             // 좌우 이동키 동시에 눌렀을 때 멈추지 않도록 하는 변수

    [SerializeField]
    bool isPlatform;            // 플랫폼 체크
    BoxCollider2D box2D;
    protected override void Awake()
    {
        isPlayer = true;
        base.Awake();
        charaIndex = 1;

        box2D = GetComponent<BoxCollider2D>();
    }
    protected override void CharaInfoReset()
    {
        _SetHP              = GameManager.GM.eliteList[charaIndex - 1].hp;
        _Speed              = GameManager.GM.eliteList[charaIndex - 1].speed;
        _JumpValue          = GameManager.GM.eliteList[charaIndex - 1].jumpValue;

        atk._Damage         = GameManager.GM.eliteList[charaIndex - 1].atkDamage;
        atk._Delay          = GameManager.GM.eliteList[charaIndex - 1].atkDelay;
        atk._Length         = GameManager.GM.eliteList[charaIndex - 1].atkLength;
                            
        skill_1._Damage     = GameManager.GM.eliteList[charaIndex - 1].skill_1_Damage;
        skill_1._Delay      = GameManager.GM.eliteList[charaIndex - 1].Skill_1_Delay;
        skill_1._Length     = GameManager.GM.eliteList[charaIndex - 1].Skill_1_Length;

        knife._Damage       = GameManager.GM.eliteList[charaIndex - 1].Skill_2_Damage;
        knife._Speed        = GameManager.GM.eliteList[charaIndex - 1].speed * 3;
        knife._CoolTime     = GameManager.GM.eliteList[charaIndex - 1].Skill_2_Delay;
        knife._KnifeDesDelay= GameManager.GM.eliteList[charaIndex - 1].Skill_2_Length;
        knife._Sigma        = 0f;

        base.CharaInfoReset();
    }

    protected override void Start()
    {
        base.Start();
        isPlatform = false;
    }

    protected override void Update()
    {
        if (isMoveLock)
        {
            return;
        }

        InputKey();
    }

    void InputKey()
    {
        bool isControl = false; // 조작 기록 확인

        // 투척검 추척
        if (Input.GetKey(KCM.THROWINGKNIFE) && !knife._KnifeCool)
        {
            StartCoroutine(FireKnife());
            return;
        }

        // Z 공격
        if (Input.GetKeyDown(KCM.ATTACK_1))
        {
            setAtk = atk;
            ChangeAnim("Attack_Z");
            StartCoroutine(MoveUnlock(setAtk._Delay));
            return;
        }

        // 점프
        if (Input.GetKeyDown(KCM.JUMP) && isGround && !isJumpCool)
        { Jump(); StartCoroutine(JumpDelay()); return; }

        // 플랫폼에서 바닥으로 내려오기
        if (Input.GetKeyDown(KCM.MOVE_DOWN) && isPlatform)
            StartCoroutine(Box2DEneable());

        // 달리기 & 대쉬
        if (Input.GetKey(KCM.MOVE_RIGHT) || Input.GetKey(KCM.MOVE_LEFT))
        {
            /*  < 달리기 입력의 대략적인 흐름 >
             * 1. 이동 키 입력 확인
             * 2. 입력 방향으로 캐릭터 회전
             *      -> 레이캐스트를 앞으로 쏘기 때문에 캐릭터가 이동할 수 있도록 미리 돌림
             * 3. 전방으로 레이를 쏴서 앞에 타일맵이 있다면 이동 불가 (리턴)
             * 4. 양방향 입력 시 가장 최근에 입력된 방향으로 이동하도록 함
             *      -> 대쉬 (양방향)
             * 5. 한방향 입력 시 입력된 방향으로 이동
             *      -> 대쉬 (한방향)   
             */

            float h = Input.GetAxisRaw("Horizontal");
            Rotate(h);

            Debug.DrawRay(transform.position, transform.right * 0.05f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.05f, LayerMask.GetMask("Ground"));
            // 레이캐스트에 성공했다면 이동 중지
            if (hit.collider != null)
            {
                box2D.size = new Vector2(0.08f, 0.25f);
                Invoke("Box2DSizeReturn", 0.05f);
                return;
            }

            // 두 개 다 눌렸을 때 기존 방향의 반대로 이동되도록
            if (Input.GetKey(KCM.MOVE_RIGHT) && Input.GetKey(KCM.MOVE_LEFT))
            {
                // 동시 입력 중 대쉬
                if (Input.GetKey(KCM.MOVE_DASH))
                { Dash(-setH); return; }

                // 일반 이동
                Move(-setH);
                Rotate(-setH);
                return;
            }

            if (Input.GetKey(KCM.MOVE_DASH))
            { Dash(h); return; }    // 대쉬

            // 일반 이동
            Rotate(h);
            Move(h);
            setH = h;

            isControl = true;
        }

        // 조작 없음
        if (!isControl) ChangeAnim(0); return;
    }

    IEnumerator Box2DEneable()
    {
        box2D.isTrigger = true;
        yield return new WaitForSeconds(0.5f);

        isPlatform = false;

        box2D.isTrigger = false;
    }

    // 벽 끼임 방지로 콜라이더 크기 줄인거 원상복구
    void Box2DSizeReturn()
    {
        // 플레이어 벽 끼임 방지
        box2D.size = new Vector2(0.09f, 0.25f);
    }

    IEnumerator FireKnife()
    {
        knife._KnifeCool = true;

        // 생성 부분
        int rot = setH < 0 ? 0 : -1;
        GameObject temp = Instantiate(knife.knife,
                          transform.position,
                          Quaternion.Euler(0, 180 * rot, 0));
        temp.GetComponent<ThrowingKnife>().damage = knife._Damage;

        // 발사 부분
        if (rot == 0) rot = 1;
        float ran = Random.Range(-knife._Sigma, knife._Sigma);
        temp.GetComponent<Rigidbody2D>().AddForce(
            new Vector3(rot * -1 * knife._Speed, ran, 0),
            ForceMode2D.Impulse);

        yield return new WaitForSeconds(knife._CoolTime);

        Destroy(temp, knife._KnifeDesDelay);
        knife._KnifeCool = false;
    }

    IEnumerator JumpDelay()
    {
        isJumpCool = true;

        yield return new WaitForSeconds(jumpDelay);

        isJumpCool = false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // 플랫폼을 올라왔을 시 내려올 수 있도록 참으로 변경
        if (collision.gameObject.CompareTag("Platform"))
        {
            isPlatform = true;
        }
    }

    // 플레이어의 isTrigger를 건드려서 내려오는거기때문에 트리거로 받음
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            box2D.isTrigger = false;
            isPlatform = false;
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);

        // 플랫폼에서 내려왔을 시 바닥으로 꺼지지 않게 거짓으로 변경
        if (collision.gameObject.CompareTag("Platform"))
        {
            isPlatform = false;
        }
    }
}

[System.Serializable]
public struct Knife
{
    public GameObject knife;    // 투척검
    private int   damage;        // 투척검 데미지
    private float speed;         // 투척검 속도
    private float coolTime;      // 투척검 쿨타임
    private float knifeDesDelay; // 투척검 자폭 딜레이
    private float sigma;         // 투척검 시그마값

    private bool knifeCool;      // 투척검 쿨타임 중

    public int _Damage
    {
        get { return damage; }
        set { damage = value; }
    }   // 데미지 관리
    public float _Speed
    {
        get { return speed; }
        set { speed = value; }
    }   // 투척검 속도 관리
    public float _CoolTime
    {
        get { return coolTime; }
        set { coolTime = value; }
    }   // 공격쿨타임 관리
    public float _KnifeDesDelay
    {
        get { return knifeDesDelay; }
        set { knifeDesDelay = value; }
    }   // 투척검 자폭시관 관리
    public float _Sigma
    {
        get { return sigma; }
        set { sigma = value; }
    }   // 분산도 관리
    public bool _KnifeCool
    {
        get { return knifeCool; }
        set { knifeCool = value; }
    }   // 투척검 쿨타임중 관리
}

[System.Serializable]
public struct KeyCodeManager
{
    public KeyCode THROWINGKNIFE;
    public KeyCode ATTACK_1;
    public KeyCode ATTACK_2;
    public KeyCode JUMP;
    public KeyCode MOVE_LEFT;
    public KeyCode MOVE_RIGHT;
    public KeyCode MOVE_DOWN;
    public KeyCode MOVE_DASH;
}
