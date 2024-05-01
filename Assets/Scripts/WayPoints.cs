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

    // 플레이어가 이 영역에 진입했을 때 호출될 메소드
    public void OnTriggerEnter(Collider other)
    {
        float randomValue = Random.value;

        Walk playerMovement = other.GetComponent<Walk>();
        if (playerMovement != null)
        {
            if(randomValue < wps01percent)
            {
                playerMovement.SetWaypoints(waypoints01); // 플레이어에게 웨이포인트 전달
            }
            else if(randomValue >= wps01percent && randomValue < wps02percent)
            {
                playerMovement.SetWaypoints(waypoints02); // 플레이어에게 웨이포인트 전달
            }
            else if(randomValue >= wps02percent && randomValue < wps03percent)
            {
                playerMovement.SetWaypoints(waypoints03); // 플레이어에게 웨이포인트 전달
            }
        }
    }
}
