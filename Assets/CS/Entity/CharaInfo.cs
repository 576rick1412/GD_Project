using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharaInfo : MonoBehaviour
{
    protected bool isPlayer;    // �÷��̾����� ����

    public float _HP
    {
        get
        { 
            return HP;
        }

        set
        {
            if(HP <= 0)
            {
                return;
            }   // ü���� 0 ���϶�� ����

            HP -= value;
            if (HP <= 0) Die();
            else Hit();
        }
    }         // ü�� ���� ������Ƽ
    [Header("ĳ���� �⺻ ����")]
    public float setHP;         // HP �ִ�ġ ����
    [HideInInspector]
    public float HP;            // ĳ���� HP
    public float speed;         // ĳ���� �ӵ�
    public float jumpValue;     // ĳ���� ���� ����

    protected bool isGround;    // �ٴ� üũ && ���� üũ
    protected bool isMoveLock;  // ĳ���� �̵� ���

    [Serializable]
    public struct Attack
    {
        public int damage;      // ���� ������
        public float delay;     // ���� ������
        public float length;    // ���� ����
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

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        HP = setHP;     // ü�� �ʱ�ȭ

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

        ChangeAnim("Jump");  // ���� �ִϸ��̼����� ����
    }   // ĳ���� ����
    protected virtual void Dash(float h)
    {
        if (h < 0) rigid.velocity = Vector2.left * speed * 2f;
        else rigid.velocity = Vector2.right * speed * 2f;

        StartCoroutine(MoveUnlock(0.8f));
    }   // ĳ���� �뽬

    // Cast�Լ����� �ִϸ��̼� �̺�Ʈ���� ���
    public virtual void Cast_S()
    {
        Debug.DrawRay(transform.position, transform.right * setAtk.length, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, setAtk.length, mask);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<CharaInfo>()._HP = setAtk.damage;
            Debug.Log("��");
        }

        // ������ ���� �������� ������ ����
        //StartCoroutine(MoveUnlock(setAtk.delay));
    }

    public virtual void Cast_M()
    {
        Debug.DrawRay(transform.position, transform.right * setAtk.length, Color.red);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, setAtk.length, mask);

        if (hits != null) return;

        foreach (var i in hits)
        {
            i.collider.gameObject.GetComponent<CharaInfo>()._HP = setAtk.damage;
        }

        // ������ ���� �������� ������ ����
        //StartCoroutine(MoveUnlock(setAtk.delay));
    }

    protected virtual void Die()
    {
        ChangeAnim("Die");
    }

    public virtual void Hit()
    {
        Debug.Log("�ǰ�");
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
