using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walk : MonoBehaviour
{
    private Transform[] waypoints; // �÷��̾ ���� ��� ����Ʈ��
    private int waypointIndex = 0; // ���� ��ǥ ����Ʈ �ε���
    public float moveSpeed = 2f; // �÷��̾� �̵� �ӵ�
    public float thresholdDistance = 0.5f; // ��������Ʈ ��ó �������� ������ �Ÿ�
    private Animator animator;

    private bool move = false; // �÷��̾ �̵� ������ ����

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
        waypointIndex = 0; // ��� �ε����� 0���� �ʱ�ȭ
        move = true; // �̵� ����
        animator.SetBool("isWalking", true);
    }

    private void MoveAlongPath()
    {
        // ���� ��ǥ ����Ʈ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, moveSpeed * Time.deltaTime);

        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[waypointIndex].position);

        // ���� ������ �ٶ󺸰� �ϱ�
        if (waypointIndex < waypoints.Length)
        {
            // ���� ��ġ���� ���� ��������Ʈ�� �ٶ󺸵��� ȸ��
            transform.LookAt(waypoints[waypointIndex].position);
        }

        // ��ǥ ����Ʈ�� �����ߴ��� Ȯ��
        if (distanceToWaypoint <= thresholdDistance)
        {
            waypointIndex += 1; // ���� ����Ʈ�� �ε��� ����
            if (waypointIndex == waypoints.Length)
            {
                move = false; // ��� ����Ʈ�� ������ �̵� ����
                // �������� �������� ���� �߰� �ൿ�� ���⿡ ����
                animator.SetBool("isWalking", false);
            }
        }
    }
}
