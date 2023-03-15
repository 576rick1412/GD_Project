using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaInfo
{
    public Attack[] atk;


    float setH = 1;     // �¿� �̵�Ű ���ÿ� ������ �� ������ �ʵ��� �ϴ� ����

    public override void Awake()
    {
        base.Awake();

        mask = LayerMask.GetMask("Enemy");
    }

    public override void Start()
    {
        
    }

    public override void Update()
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
            // �� �� �� ������ �� ���� ������ �ݴ�� �̵��ǵ���
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                // ���� �Է� �� �뽬
                if (Input.GetKey(KeyCode.LeftShift))
                { Dash(-setH); return; }    
                
                // �Ϲ� �̵�
                MoveRot(-setH); return; 
            }

            float h = Input.GetAxisRaw("Horizontal");

            if (Input.GetKey(KeyCode.LeftShift))
            { Dash(h); return; }    // �뽬

            // �Ϲ� �̵�
            MoveRot(h); setH = h;

            isControl = true;
        }

        // ���� ����
        if(!isControl) ChangeAnim(0); return;
    }
}
