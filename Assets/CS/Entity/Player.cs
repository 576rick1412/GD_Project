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

    public Attack[] atk;

    float setH = 1;             // �¿� �̵�Ű ���ÿ� ������ �� ������ �ʵ��� �ϴ� ����

    [SerializeField]
    bool isPlatform;            // �÷��� üũ
    BoxCollider2D box2D;
    protected override void Awake()
    {
        isPlayer = true;
        base.Awake();

        box2D = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        isPlatform = false;
    }

    protected override void Update()
    {
        if (!isMoveLock) InputKey();
    }

    void InputKey()
    {
        bool isControl = false; // ���� ��� Ȯ��

        // ��ô�� ��ô
        if (Input.GetKey(KCM.THROWINGKNIFE) && !knife.knifeCool)
        {
            StartCoroutine(FireKnife());
            return;
        }

        // Z ����
        if (Input.GetKeyDown(KCM.ATTACK_1))
        {
            setAtk = atk[0];
            ChangeAnim("Attack_Z");
            StartCoroutine(MoveUnlock(setAtk.delay));
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
        knife.knifeCool = true;

        int rot = transform.rotation.y < 0 ? -180 : 1;

        GameObject temp = Instantiate(knife.knife,
                          transform.position,
                          Quaternion.Euler(0, 180 * rot, 0));
        temp.GetComponent<ThrowingKnife>().damage = knife.knifeDamage;

        rot = rot == -180 ? -1 : 1;
        temp.GetComponent<Rigidbody2D>().AddForce(
            Vector2.right * rot * knife.knifeSpeed,
            ForceMode2D.Impulse);

        yield return new WaitForSeconds(knife.knifeTime);

        knife.knifeCool = false;

        Destroy(temp, knife.knifeDesDelay);
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
    public float knifeSpeed;    // ��ô�� �ӵ�
    public float knifeTime;     // ��ô�� ������
    public int knifeDamage;     // ��ô�� ������
    public int knifeDesDelay;   // ��ô�� ���� ������

    [HideInInspector]
    public bool knifeCool;      // ��ô�� ��Ÿ�� ��
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
