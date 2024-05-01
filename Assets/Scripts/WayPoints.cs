using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] waypoints01; // 이동할 경로 포인트들
    public float wps01percent;

    public Transform[] waypoints02;
    public float wps02percent;

    public Transform[] waypoints03;
    public float wps03percent;

    public float waitTime = 0.1f; // 걷기 전 대기시간

    private Walk playerMovement; // Walk 클래스를 참조할 변수

    // 플레이어가 이 영역에 진입했을 때 호출될 메소드
    public void OnTriggerEnter(Collider other)
    {
        float randomValue = Random.value;

        playerMovement = other.GetComponent<Walk>(); // Walk 컴포넌트 참조
        if (playerMovement != null)
        {
            // 대기 시간 후에 이동 메소드 호출
            Invoke("MoveAfterWait", waitTime);
        }
    }

    // 대기 후에 호출될 함수
    void MoveAfterWait()
    {
        float randomValue = Random.value;

        if (randomValue < wps01percent)
        {
            playerMovement.SetWaypoints(waypoints01); // 플레이어에게 웨이포인트 전달
        }
        else if (randomValue >= wps01percent && randomValue < wps02percent)
        {
            playerMovement.SetWaypoints(waypoints02); // 플레이어에게 웨이포인트 전달
        }
        else if (randomValue >= wps02percent && randomValue < wps03percent)
        {
            playerMovement.SetWaypoints(waypoints03); // 플레이어에게 웨이포인트 전달
        }
    }
}
