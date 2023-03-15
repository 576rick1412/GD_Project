using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaInfo
{
    public Attack[] atk;


    float setH = 1;     // �¿� �̵�Ű ���ÿ� ������ �� ������ �ʵ��� �ϴ� ����

    protected override void Awake()
    {
        base.Awake();

        mask = LayerMask.GetMask("Enemy");
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        if(!isMoveLock) InputKey();
    }

    void InputKey()
    {
        bool isControl = false; // ���� ��� Ȯ��

        // ����
        if(Input.GetKeyDown(KeyCode.Space) && !isJumpLock)
        { Jump(); return; }

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
                Move(-setH); return; 
            }

            if (Input.GetKey(KeyCode.LeftShift))
            { Dash(h); return; }    // �뽬

            // �Ϲ� �̵�
            Move(h); setH = h;

            isControl = true;
        }

        // ���� ����
        if(!isControl) ChangeAnim(0); return;
    }
}
