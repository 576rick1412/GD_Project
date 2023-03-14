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
    protected bool isJumpLock;  // ĳ���� ���� ���
    protected bool isMoveLock;  // ĳ���� �̵� ���

    [Serializable]
    public struct Attack
    {
        public int damage;      // ���� ������
        public float delay;     // ���� ������
        public float length;    // ���� ����
    }

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
        ChangeAnim(1);  // �޸��� �ִϸ��̼����� ����

        float rot = h < 0 ? 180 : 0;
        transform.rotation = Quaternion.Euler(0, rot, 0);
    }

    public void Jump()
    {
        rigid.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
        isJumpLock = true;
    }   // ĳ���� ����
    public void Dash(float h)
    {
        if (h < 0) rigid.velocity = Vector2.left * speed * 2f;
        else rigid.velocity = Vector2.right * speed * 2f;

        StartCoroutine(MoveUnlock(0.8f));
    }   // ĳ���� �뽬

    /*
    // Cast�Լ����� �ִϸ��̼� �̺�Ʈ���� ���
    public virtual void Cast_S(int i)
    {
        Debug.DrawRay(transform.position, transform.right * atk[i].length, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, atk[i].length, mask);

        if (hit.collider != null) hit.collider.gameObject.GetComponent<CharaInfo>().Hit(atk[i].DMG);

        // ������ ���� �������� ������ ����
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
    */  // ����ĳ��Ʈ �ҷ���
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
            isJumpLock = false;
    }
}
