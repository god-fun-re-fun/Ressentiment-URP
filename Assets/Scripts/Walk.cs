using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walk : MonoBehaviour
{
    private Transform[] waypoints; // 플레이어가 따라갈 경로 포인트들
    private int waypointIndex = 0; // 현재 목표 포인트 인덱스
    public float moveSpeed = 2f; // 플레이어 이동 속도
    public float thresholdDistance = 0.5f; // 웨이포인트 근처 도착으로 인정할 거리
    private Animator animator;

    private bool move = false; // 플레이어가 이동 중인지 여부

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (move && waypointIndex <= waypoints.Length - 1)
        {
            MoveAlongPath();
        }
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        waypointIndex = 0; // 경로 인덱스를 0으로 초기화
        move = true; // 이동 시작
        animator.SetBool("isWalking", true);
    }

    private void MoveAlongPath()
    {
        // 현재 목표 포인트로 이동
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, moveSpeed * Time.deltaTime);

        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[waypointIndex].position);

        // 진행 방향을 바라보게 하기
        if (waypointIndex < waypoints.Length)
        {
            // 현재 위치에서 다음 웨이포인트를 바라보도록 회전
            transform.LookAt(waypoints[waypointIndex].position);
        }

        // 목표 포인트에 도착했는지 확인
        if (distanceToWaypoint <= thresholdDistance)
        {
            waypointIndex += 1; // 다음 포인트로 인덱스 변경
            if (waypointIndex == waypoints.Length)
            {
                move = false; // 모든 포인트를 지나면 이동 중지
                // 목적지에 도착했을 때의 추가 행동을 여기에 구현
                animator.SetBool("isWalking", false);
            }
        }
    }
}
