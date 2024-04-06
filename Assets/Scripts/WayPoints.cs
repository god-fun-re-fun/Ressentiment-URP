using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform[] waypoints; // �̵��� ��� ����Ʈ��

    // �÷��̾ �� ������ �������� �� ȣ��� �޼ҵ�
    public void OnTriggerEnter(Collider other)
    {
        Walk playerMovement = other.GetComponent<Walk>();
        if (playerMovement != null)
        {
            playerMovement.SetWaypoints(waypoints); // �÷��̾�� ��������Ʈ ����
        }
    }
}
