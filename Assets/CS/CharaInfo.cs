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
    }         // ü�� ���� ������Ƽ
    public float setHP;         // HP �ִ�ġ ����
    [HideInInspector]
    public float HP;            // ĳ���� HP
    public float speed;         // ĳ���� �ӵ�
    public float jumpValue;     // ĳ���� ���� ����

    protected bool isJump;      // ĳ���� ����
    protected bool isJumpLock;  // ĳ���� ���� ���
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

    Rigidbody2D rigid;
    Animator anim;
    protected IEnumerator moveLockIenum;  // ����� �ڷ�ƾ ��� IEnumerator
    /* �ִϸ��̼� ���� ��ȣ
     0 : ����
     1 : �޸���
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

        ChangeAnim(1);  // �޸��� �ִϸ��̼����� ����
    }

    public void Jump()
    {
        rigid.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
        isJumpLock = true;

        ChangeAnim("Jump");  // ���� �ִϸ��̼����� ����
    }   // ĳ���� ����
    public void Dash(float h)
    {
        if (h < 0) rigid.velocity = Vector2.left * speed * 2f;
        else rigid.velocity = Vector2.right * speed * 2f;

        StartCoroutine(MoveUnlock(0.8f));

        //ChangeAnim("Jump");  // �뽬 �ִϸ��̼����� ����
    }   // ĳ���� �뽬

    
    // Cast�Լ����� �ִϸ��̼� �̺�Ʈ���� ���
    public virtual void Cast_S(int i)
    {
        Debug.DrawRay(transform.position, transform.right * setAtk.length, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, setAtk.length, mask);

        if (hit.collider != null) hit.collider.gameObject.GetComponent<CharaInfo>()._HP = setAtk.damage;

        // ������ ���� �������� ������ ����
        StartCoroutine(MoveUnlock(setAtk.delay));
    }

    public virtual void Cast_M(int i)
    {
        Debug.DrawRay(transform.position, transform.right * setAtk.length, Color.red);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.right, setAtk.length, mask);

        if (hit != null) return;
        for (int k = 0; k < hit.Length; k++) hit[i].collider.gameObject.GetComponent<CharaInfo>()._HP = setAtk.damage;

        // ������ ���� �������� ������ ����
        StartCoroutine(MoveUnlock(setAtk.delay));
    }

    public virtual void Die()
    {
        Debug.Log("���");
    }

    public virtual void Hit()
    {
        Debug.Log("�ǰ�");
    }


    // �ִϸ��̼� ���� �Լ�
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
