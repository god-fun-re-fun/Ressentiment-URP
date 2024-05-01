using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] waypoints01; // �̵��� ��� ����Ʈ��
    public float wps01percent;

    public Transform[] waypoints02;
    public float wps02percent;

    public Transform[] waypoints03;
    public float wps03percent;

    // �÷��̾ �� ������ �������� �� ȣ��� �޼ҵ�
    public void OnTriggerEnter(Collider other)
    {
        float randomValue = Random.value;

        Walk playerMovement = other.GetComponent<Walk>();
        if (playerMovement != null)
        {
            if(randomValue < wps01percent)
            {
                playerMovement.SetWaypoints(waypoints01); // �÷��̾�� ��������Ʈ ����
            }
            else if(randomValue >= wps01percent && randomValue < wps02percent)
            {
                playerMovement.SetWaypoints(waypoints02); // �÷��̾�� ��������Ʈ ����
            }
            else if(randomValue >= wps02percent && randomValue < wps03percent)
            {
                playerMovement.SetWaypoints(waypoints03); // �÷��̾�� ��������Ʈ ����
            }
        }
    }
}
