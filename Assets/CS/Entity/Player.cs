using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaInfo
{
    public Attack[] atk;

    public float setH = 1;     // �¿� �̵�Ű ���ÿ� ������ �� ������ �ʵ��� �ϴ� ����
    

    [SerializeField]
    bool isPlatform;    // �÷��� üũ
    BoxCollider2D box2D;
    protected override void Awake()
    {
        base.Awake();

        mask = LayerMask.GetMask("Enemy");

        box2D = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        isPlatform = false;

        int x = -10;
    }

    protected override void Update()
    {
        if(!isMoveLock) InputKey();
    }

    void InputKey()
    {
        bool isControl = false; // ���� ��� Ȯ��

        // ����
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        { Jump(); return; }

        // �÷������� �ٴ����� ��������
        if(Input.GetKeyDown(KeyCode.DownArrow) && isPlatform)
            StartCoroutine(Box2DEneable());

        // �޸���
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            /*  < �޸��� �Է��� �뷫���� �帧 >
             * 1. �̵� Ű �Է� Ȯ��
             * 2. �Է¿� ���� ĳ���� ���� �̵�
             *      -> ����ĳ��Ʈ�� ������ ��� ������ ĳ���Ͱ� �̵��� �� �ֵ��� �̸� ����
             * 3. �������� ���̸� ���� �տ� Ÿ�ϸ��� �ִٸ� �̵� �Ұ� (����)
             * 4. ����� �Է� �� ���� �ֱٿ� �Էµ� �������� �̵��ϵ��� ��
             *      -> �뽬 (�����)
             * 5. �ѹ��� �Է� �� �Էµ� �������� �̵�
             *      -> �뽬 (�ѹ���)   
             */

            float h = Input.GetAxisRaw("Horizontal");
            Rotate(h);

            Debug.DrawRay(transform.position, transform.right * 0.1f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.1f, LayerMask.GetMask("Ground"));
            // ����ĳ��Ʈ�� �����ߴٸ� �̵� ����
            if (hit.collider != null) return;

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
        if(!isControl) ChangeAnim(0); return;
    }

    IEnumerator Box2DEneable()
    {
        box2D.isTrigger = true;
        yield return new WaitForSeconds(0.5f);

        isPlatform = false;

        box2D.isTrigger = false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // �÷����� �ö���� �� ������ �� �ֵ��� ������ ����
        if (collision.gameObject.CompareTag("Platform")) isPlatform = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
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
        if (collision.gameObject.CompareTag("Platform")) isPlatform = false;
    }
}
