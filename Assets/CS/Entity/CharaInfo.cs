using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharaInfo : MonoBehaviour
{
    protected bool isPlayer;    // �÷��̾����� ����

    // ---------------------------
    // ����
    // ---------------------------
    [Header("ĳ���� �⺻ ����")]
    float setHP;         // HP �ִ�ġ ����
    float hp;            // ĳ���� HP
    float speed;         // ĳ���� �ӵ�
    float jumpValue;     // ĳ���� ���� ����

    // ---------------------------
    // ������Ƽ
    // ---------------------------
    public float _HP
    {
        get { return hp; }

        set
        {
            if (hp <= 0)
            {
                return;
            }   // ü���� 0 ���϶�� ����

            hp -= value;
            if (hp <= 0) Die();
            else Hit();
        }
    }             // ü�� ����
    protected float _SetHP          // �ִ� ü�� ����
    {
        get { return setHP; }
        set { setHP = value; }
    }
    protected float _Speed
    {
        get { return speed; }
        set { speed = value; }
    }       // ���ǵ� ����
    protected float _JumpValue
    {
        get { return jumpValue; }
        set { jumpValue = value; }
    }   // ���� ����


    // ---------------------------
    // ��Ÿ
    // ---------------------------
    protected bool isGround;    // �ٴ� üũ && ���� üũ
    protected bool isMoveLock;  // ĳ���� �̵� ���

    [Serializable]
    public class Attack
    {
        // ����
        int damage;      // ���� ������
        float delay;     // ���� ������
        float length;    // ���� ����

        // ������Ƽ
        public int _Damage
        {
            get { return damage; }
            set { damage = value; }
        }   // ������ ����
        public float _Delay
        {
            get { return delay; }
            set { delay = value; }
        }   // ������ ����
        public float _Length
        {
            get { return length; }
            set { length = value; }
        }   // ���� ���� ����
    }

    protected Attack setAtk;
    protected LayerMask mask;

    protected Rigidbody2D rigid;
    protected Animator anim;
    protected IEnumerator moveLockIenum;  // ����� �ڷ�ƾ ��� IEnumerator
    /* �ִϸ��̼� ���� ��ȣ
     0 : ����
     1 : �޸���
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

        hp = setHP;     // ü�� �ʱ�ȭ

        // �ǰ� ���� ����
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

        ChangeAnim(1);  // �޸��� �ִϸ��̼����� ����
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
        ChangeAnim("Jump");  // ���� �ִϸ��̼����� ����
    }   // ĳ���� ����
    protected virtual void Dash(float h)
    {
        if (h < 0) rigid.velocity = Vector2.left * speed * 2f;
        else rigid.velocity = Vector2.right * speed * 2f;

        ChangeAnim(0);
        ChangeAnim("Dash");
        StartCoroutine(MoveUnlock(0.8f));
    }   // ĳ���� �뽬

    // Cast�Լ����� �ִϸ��̼� �̺�Ʈ���� ���
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


    // �ִϸ��̼� ���� �Լ�
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
