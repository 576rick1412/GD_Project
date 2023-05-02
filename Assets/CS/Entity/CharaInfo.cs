using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharaInfo : MonoBehaviour
{
    protected bool isPlayer;    // 플레이어인지 여부

    // ---------------------------
    // 변수
    // ---------------------------
    [Header("캐릭터 기본 설정")]
    float setHP;         // HP 최대치 저장
    float hp;            // 캐릭터 HP
    float speed;         // 캐릭터 속도
    float jumpValue;     // 캐릭터 점프 높이

    // ---------------------------
    // 프로퍼티
    // ---------------------------
    public float _HP
    {
        get { return hp; }

        set
        {
            if (hp <= 0)
            {
                return;
            }   // 체력이 0 이하라면 리턴

            hp -= value;
            if (hp <= 0) Die();
            else Hit();
        }
    }             // 체력 관리
    protected float _SetHP          // 최대 체력 관리
    {
        get { return setHP; }
        set { setHP = value; }
    }
    protected float _Speed
    {
        get { return speed; }
        set { speed = value; }
    }       // 스피드 관리
    protected float _JumpValue
    {
        get { return jumpValue; }
        set { jumpValue = value; }
    }   // 점프 관리


    // ---------------------------
    // 기타
    // ---------------------------
    protected bool isGround;    // 바닥 체크 && 점프 체크
    protected bool isMoveLock;  // 캐릭터 이동 잠금

    [Serializable]
    public class Attack
    {
        // 변수
        int damage;      // 공격 데미지
        float delay;     // 공격 딜레이
        float length;    // 공격 길이

        // 프로퍼티
        public int _Damage
        {
            get { return damage; }
            set { damage = value; }
        }   // 데미지 관리
        public float _Delay
        {
            get { return delay; }
            set { delay = value; }
        }   // 딜레이 관리
        public float _Length
        {
            get { return length; }
            set { length = value; }
        }   // 공격 길이 관리
    }

    protected Attack setAtk;
    protected LayerMask mask;

    protected Rigidbody2D rigid;
    protected Animator anim;
    protected IEnumerator moveLockIenum;  // 무브락 코루틴 담는 IEnumerator
    /* 애니메이션 지정 번호
     0 : 정지
     1 : 달리기
     */

    protected virtual void CharaReset(
        float setHp,float speed,float jumpValue )
    {
        _SetHP = setHp;
        _Speed = speed;
        _JumpValue = jumpValue;
    }

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        hp = setHP;     // 체력 초기화

        // 피격 관련 설정
        if(isPlayer)
        {
            mask = LayerMask.GetMask("Enemy");
        }
        else
        {
            mask = LayerMask.GetMask("Player");
        }

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

        isGround = false;
        ChangeAnim("Jump");  // 점프 애니메이션으로 변경
    }   // 캐릭터 점프
    protected virtual void Dash(float h)
    {
        if (h < 0) rigid.velocity = Vector2.left * speed * 2f;
        else rigid.velocity = Vector2.right * speed * 2f;

        ChangeAnim(0);
        ChangeAnim("Dash");
        StartCoroutine(MoveUnlock(0.8f));
    }   // 캐릭터 대쉬

    // Cast함수들은 애니메이션 이벤트에서 사용
    public virtual void Cast_S()
    {
        Debug.DrawRay(transform.position, transform.right * setAtk._Length, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, setAtk._Length, mask);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<CharaInfo>()._HP = setAtk._Damage;
        }
    }
    public virtual void Cast_M()
    {
        Debug.DrawRay(transform.position, transform.right * setAtk._Length, Color.red);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, setAtk._Length, mask);

        if (hits == null) return;

        foreach (var i in hits)
        {
            i.collider.gameObject.GetComponent<CharaInfo>()._HP = setAtk._Damage;
        }
    }

    protected virtual void Die()
    {
        isMoveLock = true;
        StopAllCoroutines();
        ChangeAnim("Die");
    }

    public virtual void Hit()
    {
        anim.SetTrigger("Hit");
    }


    // 애니메이션 설정 함수
    protected virtual void ChangeAnim(int i) { anim.SetInteger("action", i); }
    protected virtual void ChangeAnim(string name, bool isValue) { anim.SetBool(name, isValue); }
    protected virtual void ChangeAnim(string name) { anim.SetTrigger(name); }

    protected IEnumerator MoveUnlock(float time)
    {
        isMoveLock = true;

        yield return new WaitForSeconds(time);
        isMoveLock = false;

        rigid.velocity = Vector2.zero;

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
