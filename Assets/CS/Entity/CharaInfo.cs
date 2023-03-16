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

    protected bool isGround;    // 바닥 체크 && 점프 체크
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

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        isMoveLock = false;
    }

    protected virtual void Start()
    {
        isGround = true;
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void Move(float h)
    {
        transform.position += new Vector3(h, 0, 0) * speed * Time.deltaTime;

        ChangeAnim(1);  // 달리기 애니메이션으로 변경
    }

    protected virtual void Rotate(float h)
    {
        float rot = h < 0 ? 180 : 0;
        transform.rotation = Quaternion.Euler(0, rot, 0);
    }

    protected virtual void Jump()
    {
        rigid.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);

        ChangeAnim("Jump");  // 점프 애니메이션으로 변경
    }   // 캐릭터 점프
    protected virtual void Dash(float h)
    {
        if (h < 0) rigid.velocity = Vector2.left * speed * 2f;
        else rigid.velocity = Vector2.right * speed * 2f;

        StartCoroutine(MoveUnlock(0.8f));
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

    protected virtual void Die()
    {
        Debug.Log("사망");
    }

    public virtual void Hit()
    {
        Debug.Log("피격");
    }


    // 애니메이션 설정 함수
    protected virtual void ChangeAnim(int i) { anim.SetInteger("action", i); }
    protected virtual void ChangeAnim(string name, bool isValue) { anim.SetBool(name, isValue); }
    protected virtual void ChangeAnim(string name) { anim.SetTrigger(name); }

    IEnumerator MoveUnlock(float time)
    {
        isMoveLock = true;

        yield return new WaitForSeconds(time);
        isMoveLock = false;

        yield return null;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGround = true;
            ChangeAnim("isGround", isGround);
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGround = false;
            ChangeAnim("isGround", isGround);
        }
    }
}
