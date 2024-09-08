using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltMove : MonoBehaviour
{
    float speed;
    Rigidbody rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //���� �������� �ִϸ��̼��� �ӵ��� ������                                                         x,z 0.2�� ���Ͻ� 0.35��
        speed = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / animator.GetCurrentAnimatorStateInfo(0).length * 0.35F;
    }

    void FixedUpdate()
    {
        // ���� ������Ʈ�� ���� ����
        Vector3 forwardDirection = transform.right;

        Vector3 pos = rb.position;

        // ��Ʈ�� �̵� ���� ��� (���� ���� ����)
        rb.position += forwardDirection * speed * Time.fixedDeltaTime;

        // �̵� ����
        rb.MovePosition(pos);
    }
}