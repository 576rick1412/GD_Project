using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaInfo
{
    [Header("����Ű ����")]
    public KeyCodeManager KCM;  // ����Ű �Ŵ���

    [Header("��ô��")]
    public Knife knife;

    [Header("�÷��̾� ����")]
    public bool isJumpCool;     // ���� ���� ����, ������ �� ���� ����
    public float jumpDelay;     // ���� ������

    public Attack atk;
    public Attack skill_1;

    float setH = 1;             // �¿� �̵�Ű ���ÿ� ������ �� ������ �ʵ��� �ϴ� ����

    [SerializeField]
    bool isPlatform;            // �÷��� üũ
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
        bool isControl = false; // ���� ��� Ȯ��

        // ��ô�� ��ô
        if (Input.GetKey(KCM.THROWINGKNIFE) && !knife._KnifeCool)
        {
            StartCoroutine(FireKnife());
            return;
        }

        // Z ����
        if (Input.GetKeyDown(KCM.ATTACK_1))
        {
            setAtk = atk;
            ChangeAnim("Attack_Z");
            StartCoroutine(MoveUnlock(setAtk._Delay));
            return;
        }

        // ����
        if (Input.GetKeyDown(KCM.JUMP) && isGround && !isJumpCool)
        { Jump(); StartCoroutine(JumpDelay()); return; }

        // �÷������� �ٴ����� ��������
        if (Input.GetKeyDown(KCM.MOVE_DOWN) && isPlatform)
            StartCoroutine(Box2DEneable());

        // �޸��� & �뽬
        if (Input.GetKey(KCM.MOVE_RIGHT) || Input.GetKey(KCM.MOVE_LEFT))
        {
            /*  < �޸��� �Է��� �뷫���� �帧 >
             * 1. �̵� Ű �Է� Ȯ��
             * 2. �Է� �������� ĳ���� ȸ��
             *      -> ����ĳ��Ʈ�� ������ ��� ������ ĳ���Ͱ� �̵��� �� �ֵ��� �̸� ����
             * 3. �������� ���̸� ���� �տ� Ÿ�ϸ��� �ִٸ� �̵� �Ұ� (����)
             * 4. ����� �Է� �� ���� �ֱٿ� �Էµ� �������� �̵��ϵ��� ��
             *      -> �뽬 (�����)
             * 5. �ѹ��� �Է� �� �Էµ� �������� �̵�
             *      -> �뽬 (�ѹ���)   
             */

            float h = Input.GetAxisRaw("Horizontal");
            Rotate(h);

            Debug.DrawRay(transform.position, transform.right * 0.05f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.05f, LayerMask.GetMask("Ground"));
            // ����ĳ��Ʈ�� �����ߴٸ� �̵� ����
            if (hit.collider != null)
            {
                box2D.size = new Vector2(0.08f, 0.25f);
                Invoke("Box2DSizeReturn", 0.05f);
                return;
            }

            // �� �� �� ������ �� ���� ������ �ݴ�� �̵��ǵ���
            if (Input.GetKey(KCM.MOVE_RIGHT) && Input.GetKey(KCM.MOVE_LEFT))
            {
                // ���� �Է� �� �뽬
                if (Input.GetKey(KCM.MOVE_DASH))
                { Dash(-setH); return; }

                // �Ϲ� �̵�
                Move(-setH);
                Rotate(-setH);
                return;
            }

            if (Input.GetKey(KCM.MOVE_DASH))
            { Dash(h); return; }    // �뽬

            // �Ϲ� �̵�
            Rotate(h);
            Move(h);
            setH = h;

            isControl = true;
        }

        // ���� ����
        if (!isControl) ChangeAnim(0); return;
    }

    IEnumerator Box2DEneable()
    {
        box2D.isTrigger = true;
        yield return new WaitForSeconds(0.5f);

        isPlatform = false;

        box2D.isTrigger = false;
    }

    // �� ���� ������ �ݶ��̴� ũ�� ���ΰ� ���󺹱�
    void Box2DSizeReturn()
    {
        // �÷��̾� �� ���� ����
        box2D.size = new Vector2(0.09f, 0.25f);
    }

    IEnumerator FireKnife()
    {
        knife._KnifeCool = true;

        // ���� �κ�
        int rot = setH < 0 ? 0 : -1;
        GameObject temp = Instantiate(knife.knife,
                          transform.position,
                          Quaternion.Euler(0, 180 * rot, 0));
        temp.GetComponent<ThrowingKnife>().damage = knife._Damage;

        // �߻� �κ�
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

        // �÷����� �ö���� �� ������ �� �ֵ��� ������ ����
        if (collision.gameObject.CompareTag("Platform"))
        {
            isPlatform = true;
        }
    }

    // �÷��̾��� isTrigger�� �ǵ���� �������°ű⶧���� Ʈ���ŷ� ����
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

        // �÷������� �������� �� �ٴ����� ������ �ʰ� �������� ����
        if (collision.gameObject.CompareTag("Platform"))
        {
            isPlatform = false;
        }
    }
}

[System.Serializable]
public struct Knife
{
    public GameObject knife;    // ��ô��
    private int   damage;        // ��ô�� ������
    private float speed;         // ��ô�� �ӵ�
    private float coolTime;      // ��ô�� ��Ÿ��
    private float knifeDesDelay; // ��ô�� ���� ������
    private float sigma;         // ��ô�� �ñ׸���

    private bool knifeCool;      // ��ô�� ��Ÿ�� ��

    public int _Damage
    {
        get { return damage; }
        set { damage = value; }
    }   // ������ ����
    public float _Speed
    {
        get { return speed; }
        set { speed = value; }
    }   // ��ô�� �ӵ� ����
    public float _CoolTime
    {
        get { return coolTime; }
        set { coolTime = value; }
    }   // ������Ÿ�� ����
    public float _KnifeDesDelay
    {
        get { return knifeDesDelay; }
        set { knifeDesDelay = value; }
    }   // ��ô�� �����ð� ����
    public float _Sigma
    {
        get { return sigma; }
        set { sigma = value; }
    }   // �л굵 ����
    public bool _KnifeCool
    {
        get { return knifeCool; }
        set { knifeCool = value; }
    }   // ��ô�� ��Ÿ���� ����
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
