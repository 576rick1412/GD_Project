using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharaInfo : MonoBehaviour
{
    public float _HP
    {
        get
        { 
            return HP;
        }

        set
        {
            HP -= value;
            if (HP <= 0) Die();
            else Hit();
        }
    }         // 체력 관리 프로퍼티
    public float setHP;         // HP 최대치 저장
    [HideInInspector]
    public float HP;            // 캐릭터 HP
    public float speed;         // 캐릭터 속도
    public float jumpValue;     // 캐릭터 점프 높이

    protected bool isJump;      // 캐릭터 점프
    protected bool isJumpLock;  // 캐릭터 점프 잠금
    protected bool isMoveLock;  // 캐릭터 이동 잠금

    [Serializable]
    public struct Attack
    {
        public int damage;      // 공격 데미지
        public float delay;     // 공격 딜레이
        public float length;    // 공격 길이
    }

    protected Attack setAtk;
    protected LayerMask mask;

    Rigidbody2D rigid;
    Animator anim;
    protected IEnumerator moveLockIenum;  // 무브락 코루틴 담는 IEnumerator
    /* 애니메이션 지정 번호
     0 : 정지
     1 : 달리기
     */

    public virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        isJump     = false;
        isJumpLock = false;
        isMoveLock = false;
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void MoveRot(float h)
    {
        transform.position += new Vector3(h, 0, 0) * speed * Time.deltaTime;

        float rot = h < 0 ? 180 : 0;
        transform.rotation = Quaternion.Euler(0, rot, 0);

        ChangeAnim(1);  // 달리기 애니메이션으로 변경
    }

    public void Jump()
    {
        rigid.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
        isJumpLock = true;

        ChangeAnim("Jump");  // 점프 애니메이션으로 변경
    }   // 캐릭터 점프
    public void Dash(float h)
    {
        if (h < 0) rigid.velocity = Vector2.left * speed * 2f;
        else rigid.velocity = Vector2.right * speed * 2f;

        StartCoroutine(MoveUnlock(0.8f));

        //ChangeAnim("Jump");  // 대쉬 애니메이션으로 변경
    }   // 캐릭터 대쉬

    
    // Cast함수들은 애니메이션 이벤트에서 사용
    public virtual void Cast_S(int i)
    {
        Debug.DrawRay(transform.position, transform.right * setAtk.length, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, setAtk.length, mask);

        if (hit.collider != null) hit.collider.gameObject.GetComponent<CharaInfo>()._HP = setAtk.damage;

        // 공격이 나간 시점부터 딜레이 시작
        StartCoroutine(MoveUnlock(setAtk.delay));
    }

    public virtual void Cast_M(int i)
    {
        Debug.DrawRay(transform.position, transform.right * setAtk.length, Color.red);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.right, setAtk.length, mask);

        if (hit != null) return;
        for (int k = 0; k < hit.Length; k++) hit[i].collider.gameObject.GetComponent<CharaInfo>()._HP = setAtk.damage;

        // 공격이 나간 시점부터 딜레이 시작
        StartCoroutine(MoveUnlock(setAtk.delay));
    }

    public virtual void Die()
    {
        Debug.Log("사망");
    }

    public virtual void Hit()
    {
        Debug.Log("피격");
    }


    // 애니메이션 설정 함수
    public void ChangeAnim(int i) { anim.SetInteger("action", i); }
    public void ChangeAnim(string name, bool isValue) { anim.SetBool(name, isValue); }
    public void ChangeAnim(string name) { anim.SetTrigger(name); }

    IEnumerator MoveUnlock(float time)
    {
        isMoveLock = true;

        yield return new WaitForSeconds(time);
        isMoveLock = false;

        yield return null;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = true;
            ChangeAnim("isJump", isJump);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            isJumpLock = false;
            ChangeAnim("isJump", isJump);
        }
    }
}
