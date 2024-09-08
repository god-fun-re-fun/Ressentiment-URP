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
        //현재 실행중인 애니메이션의 속도를 가져옴                                                         x,z 0.2로 줄일시 0.35배
        speed = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / animator.GetCurrentAnimatorStateInfo(0).length * 0.35F;
    }

    void FixedUpdate()
    {
        // 현재 오브젝트의 전방 방향
        Vector3 forwardDirection = transform.right;

        Vector3 pos = rb.position;

        // 벨트의 이동 벡터 계산 (전방 방향 기준)
        rb.position += forwardDirection * speed * Time.fixedDeltaTime;

        // 이동 적용
        rb.MovePosition(pos);
    }
}