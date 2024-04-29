using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // �� �Լ��� �ٸ� ������Ʈ�� Collider�� �� ������Ʈ�� Trigger�� ���� �� �ڵ����� ȣ��˴ϴ�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1") || other.CompareTag("P2") || other.CompareTag("P3"))
        {
            // 'other'�� Ʈ���� ������ ���� ������Ʈ�� Collider�Դϴ�.
            // ���⼭�� �ܼ��� ������� �ϱ� ���� Destroy �Լ��� ����մϴ�.
            Destroy(other.gameObject);
        }
    }
}
