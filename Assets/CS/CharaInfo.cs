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
    protected bool isJumpLock;  // 캐릭터 점프 잠금
    protected bool isMoveLock;  // 캐릭터 이동 잠금

    [Serializable]
    public struct Attack
    {
        public int damage;      // 공격 데미지
        public float delay;     // 공격 딜레이
        public float length;    // 공격 길이
    }

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
        ChangeAnim(1);  // 달리기 애니메이션으로 변경

        float rot = h < 0 ? 180 : 0;
        transform.rotation = Quaternion.Euler(0, rot, 0);
    }

    public void Jump()
    {
        rigid.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
        isJumpLock = true;
    }   // 캐릭터 점프
    public void Dash(float h)
    {
        if (h < 0) rigid.velocity = Vector2.left * speed * 2f;
        else rigid.velocity = Vector2.right * speed * 2f;

        StartCoroutine(MoveUnlock(0.8f));
    }   // 캐릭터 대쉬

    /*
    // Cast함수들은 애니메이션 이벤트에서 사용
    public virtual void Cast_S(int i)
    {
        Debug.DrawRay(transform.position, transform.right * atk[i].length, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, atk[i].length, mask);

        if (hit.collider != null) hit.collider.gameObject.GetComponent<CharaInfo>().Hit(atk[i].DMG);

        // 공격이 나간 시점부터 딜레이 시작
        StopCoroutine("MoveUnlock");
        StartCoroutine(MoveUnlock(atk[i].delay));
    }
    public virtual void Cast_M(int i)
    {
        Debug.DrawRay(transform.position, transform.right * atk[i].length, Color.red);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.right, atk[i].length, mask);

        if (hit != null) return;
        for (int k = 0; k < hit.Length; k++) hit[i].collider.gameObject.GetComponent<CharaInfo>().Hit(atk[i].DMG);
    }
    */  // 레이캐스트 불러옴
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
            isJumpLock = false;
    }
}
