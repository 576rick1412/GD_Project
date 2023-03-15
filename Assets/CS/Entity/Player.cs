using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaInfo
{
    public Attack[] atk;


    float setH = 1;     // 좌우 이동키 동시에 눌렀을 때 멈추지 않도록 하는 변수

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
        bool isControl = false; // 조작 기록 확인

        // 점프
        if(Input.GetKeyDown(KeyCode.Space) && !isJumpLock)
        { Jump(); return; }

        // 달리기
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            /*  < 달리기 입력의 대략적인 흐름 >
             * 1. 이동 키 입력 확인
             * 2. 입력에 따른 캐릭터 방향 이동
             *      -> 레이캐스트를 앞으로 쏘기 때문에 캐릭터가 이동할 수 있도록 미리 돌림
             * 3. 전방으로 레이를 쏴서 앞에 타일맵이 있다면 이동 불가 (리턴)
             * 4. 양방향 입력 시 가장 최근에 입력된 방향으로 이동하도록 함
             *      -> 대쉬 (양방향)
             * 5. 한방향 입력 시 입력된 방향으로 이동
             *      -> 대쉬 (한방향)   
             */

            float h = Input.GetAxisRaw("Horizontal");
            Rotate(h);

            Debug.DrawRay(transform.position, transform.right * 0.1f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.1f, LayerMask.GetMask("Ground"));
            // 레이캐스트에 성공했다면 이동 중지
            if (hit.collider != null) return;

            // 두 개 다 눌렸을 때 기존 방향의 반대로 이동되도록
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                // 동시 입력 중 대쉬
                if (Input.GetKey(KeyCode.LeftShift))
                { Dash(-setH); return; }    
                
                // 일반 이동
                Move(-setH); return; 
            }

            if (Input.GetKey(KeyCode.LeftShift))
            { Dash(h); return; }    // 대쉬

            // 일반 이동
            Move(h); setH = h;

            isControl = true;
        }

        // 조작 없음
        if(!isControl) ChangeAnim(0); return;
    }
}
