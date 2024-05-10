using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrain : MonoBehaviour
{
    public Transform position1; // 시작 위치
    public Transform position2; // 중간 위치
    public Transform position3; // 최종 위치
    public float speed = 1.0f; // 이동 속도
    public float waitTime = 30.0f; // 위치 2에서 대기하는 시간(초)

    void Start()
    {
        transform.position = position1.position;
        // 시작할 때 이동 코루틴을 시작합니다.
        StartCoroutine(MoveBetweenPositions());
    }

    IEnumerator MoveBetweenPositions()
    {
        // 무한 루프로 반복 실행합니다.
        while (true)
        {
            // 위치 1에서 위치 2로 이동
            yield return MoveToPosition(position2.position);
            // 위치 2에서 잠시 대기
            yield return new WaitForSeconds(waitTime);
            // 위치 2에서 위치 3으로 이동
            yield return MoveToPosition(position3.position);
            // 위치 3에서 다시 위치 1로 이동
            transform.position = position1.position;
        }
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        // 현재 위치에서 목표 위치까지 이동합니다.
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
}
