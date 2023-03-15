using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaInfo
{
    public Attack[] atk;


    float setH = 1;     // 좌우 이동키 동시에 눌렀을 때 멈추지 않도록 하는 변수

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
        bool isControl = false; // 조작 기록 확인

        // 점프
        if(Input.GetKeyDown(KeyCode.Space) && !isJumpLock)
        { Jump(); return; }

        // 달리기
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            // 두 개 다 눌렸을 때 기존 방향의 반대로 이동되도록
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                // 동시 입력 중 대쉬
                if (Input.GetKey(KeyCode.LeftShift))
                { Dash(-setH); return; }    
                
                // 일반 이동
                MoveRot(-setH); return; 
            }

            float h = Input.GetAxisRaw("Horizontal");

            if (Input.GetKey(KeyCode.LeftShift))
            { Dash(h); return; }    // 대쉬

            // 일반 이동
            MoveRot(h); setH = h;

            isControl = true;
        }

        // 조작 없음
        if(!isControl) ChangeAnim(0); return;
    }
}
