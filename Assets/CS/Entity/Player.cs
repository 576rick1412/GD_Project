using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaInfo
{
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

        // Z ����
        if(Input.GetKeyDown(KeyCode.Z))
        {
            setAtk = atk[0];
            ChangeAnim("Attack_Z");
            StartCoroutine(MoveUnlock(setAtk.delay));
            return;
        }
        
        // ����
        if (Input.GetKeyDown(KeyCode.Space) && isGround && !isJumpCool)
        { Jump(); StartCoroutine(JumpCount()); return; }

        // �÷������� �ٴ����� ��������
        if(Input.GetKeyDown(KeyCode.DownArrow) && isPlatform)
            StartCoroutine(Box2DEneable());

        // �޸��� & �뽬
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
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
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                // ���� �Է� �� �뽬
                if (Input.GetKey(KeyCode.LeftShift))
                { Dash(-setH); return; }    
                
                // �Ϲ� �̵�
                Move(-setH);
                Rotate(-setH);
                return; 
            }

            if (Input.GetKey(KeyCode.LeftShift))
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

    IEnumerator JumpCount()
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

        if (collision.gameObject.CompareTag("Ground"))
        {
            box2D.isTrigger = false;
            isPlatform = false;
        }

        if (collision.gameObject.CompareTag("EnemyHead"))
        {
            // �� - �÷��̾��� X���� �������
            if (collision.gameObject.transform.position.x - transform.position.x < 0)
            {
                Debug.Log("��");
                rigid.AddForce(new Vector2( 1f, 0f) * speed, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("��");
                rigid.AddForce(new Vector2( -1f, 0f) * speed, ForceMode2D.Impulse);
            }
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
