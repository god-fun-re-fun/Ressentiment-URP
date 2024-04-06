using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] waypoints; // 이동할 경로 포인트들

    // 플레이어가 이 영역에 진입했을 때 호출될 메소드
    public void OnTriggerEnter(Collider other)
    {
        Walk playerMovement = other.GetComponent<Walk>();
        if (playerMovement != null)
        {
            playerMovement.SetWaypoints(waypoints); // 플레이어에게 웨이포인트 전달
        }
    }
}
